// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Core.Diagnostics
{
    public class TelemetryService : IHealthWriter, IDisposable
    {
        public bool IsEnabled { get; private set; }

        private Configuration _currentConfig;
        private TelemetryClient _client;

        private static TelemetryService _current;

        public static TelemetryService Current
        {
            get
            {
                if (_current == null)
                {
                    _current = new TelemetryService(Configuration.Current);
                }

                return _current;
            }

            private set
            {
                _current = value;
            }
        }

        private TelemetryService(Configuration config)
        {
            _currentConfig = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task TrackEventAsync(string eventName, Dictionary<string, string> properties = null, Dictionary<string, double> metrics = null)
        {
            await SafeExecuteAsync(() => _client.TrackEvent(eventName, properties, metrics)).ConfigureAwait(false);
        }

        public async Task TrackExceptionAsync(Exception ex, Dictionary<string, string> properties = null, Dictionary<string, double> metrics = null)
        {
            await SafeExecuteAsync(() => _client.TrackException(ex, properties, metrics)).ConfigureAwait(false);
        }

        public async Task WriteTraceAsync(TraceEventType eventType, string message, Exception ex = null)
        {
            // Trace events will not be forwarded to the remote service
            await Task.Run(() => { });
        }

        public async Task WriteExceptionAsync(Exception ex, string message = null)
        {
            var properties = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(message))
            {
                properties.Add("Additional info", message);
            }

            await TelemetryService.Current.TrackExceptionAsync(ex, properties);
        }

        public bool AllowMultipleInstances()
        {
            return false;
        }

        public void IntializeTelemetryClient(GenShell genShell)
        {
            try
            {
                _client = new TelemetryClient()
                {
                    InstrumentationKey = _currentConfig.RemoteTelemetryKey,
                };

                if (VsTelemetryIsOptedIn(genShell) && RemoteKeyAvailable())
                {
                    if (!string.IsNullOrEmpty(_currentConfig.CustomTelemetryEndpoint))
                    {
                        TelemetryConfiguration.Active.TelemetryChannel.EndpointAddress = _currentConfig.CustomTelemetryEndpoint;
                    }

                    SetSessionData();

                    _client.TrackEvent(TelemetryEvents.SessionStart);

                    IsEnabled = true;
#if DEBUG
                    TelemetryConfiguration.Active.TelemetryChannel.DeveloperMode = true;
#endif
                }
                else
                {
                    IsEnabled = false;
                    TelemetryConfiguration.Active.DisableTelemetry = true;
                }
            }
            catch (Exception ex)
            {
                IsEnabled = false;
                TelemetryConfiguration.Active.DisableTelemetry = true;

                Trace.TraceError($"Exception instantiating TelemetryClient:\n\r{ex.ToString()}");
            }
        }

        private bool VsTelemetryIsOptedIn(GenShell genShell)
        {
            var info = genShell.GetVSTelemetryInfo();

            _client.Context.Properties.Add(TelemetryProperties.VisualStudioEdition, info.VisualStudioEdition);
            _client.Context.Properties.Add(TelemetryProperties.VisualStudioExeVersion, info.VisualStudioExeVersion);
            _client.Context.Properties.Add(TelemetryProperties.VisualStudioCulture, info.VisualStudioCulture);
            _client.Context.Properties.Add(TelemetryProperties.VisualStudioManifestId, info.VisualStudioManifestId);

            return info.OptedIn;
        }

        private void SetSessionData()
        {
            // No PII tracked
            string userToTrack = Guid.NewGuid().ToString();
            string machineToTrack = Guid.NewGuid().ToString();

            _client.Context.User.Id = userToTrack;
            _client.Context.User.AccountId = userToTrack;
            _client.Context.User.AuthenticatedUserId = userToTrack;

            _client.Context.Device.Id = machineToTrack;
            _client.Context.Device.OperatingSystem = Environment.OSVersion.VersionString;

            _client.Context.Cloud.RoleInstance = TelemetryProperties.RoleInstanceName;
            _client.Context.Cloud.RoleName = TelemetryProperties.RoleInstanceName;

            _client.Context.Session.Id = Guid.NewGuid().ToString();
            _client.Context.Component.Version = GetVersion();
            _client.Context.Properties.Add(TelemetryProperties.WizardFileVersion, GetFileVersion());
        }

        public void SetContentVersionToContext(Version contentVersion)
        {
            if (contentVersion != null && _client != null && _client.Context != null && _client.Context.Properties != null)
            {
                if (!_client.Context.Properties.ContainsKey(TelemetryProperties.WizardContentVersion))
                {
                    _client.Context.Properties.Add(TelemetryProperties.WizardContentVersion, contentVersion.ToString());
                }
                else
                {
                    _client.Context.Properties[TelemetryProperties.WizardContentVersion] = contentVersion.ToString();
                }
            }
        }

        public void SetContentVsProductVersionToContext(string vsProductVersion)
        {
            if (!string.IsNullOrEmpty(vsProductVersion) && _client != null && _client.Context != null && _client.Context.Properties != null)
            {
                if (!_client.Context.Properties.ContainsKey(TelemetryProperties.VisualStudioProductVersion))
                {
                    _client.Context.Properties.Add(TelemetryProperties.VisualStudioProductVersion, vsProductVersion);
                }
                else
                {
                    _client.Context.Properties[TelemetryProperties.VisualStudioProductVersion] = vsProductVersion;
                }
            }
        }

        private async Task SafeExecuteAsync(Action action)
        {
            try
            {
                var task = Task.Run(action);

                await task;
            }
            catch (AggregateException aggEx)
            {
                foreach (var ex in aggEx.InnerExceptions)
                {
                    Trace.TraceError($"Error tracking telemetry: {ex}");
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Error tracking telemetry: {ex}");
            }
        }

        private async Task SafeExecuteAsync(Func<Task> action)
        {
            try
            {
                var task = Task.Run(action);

                await task;
            }
            catch (AggregateException aggEx)
            {
                foreach (Exception ex in aggEx.InnerExceptions)
                {
                    Trace.TraceError($"Error tracking telemetry: {ex}");
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Error tracking telemetry: {ex}");
            }
        }

        public async Task FlushAsync()
        {
            await SafeExecuteAsync(async () =>
            {
                if (_client != null)
                {
                    _client.Flush();
                    _client = null;
                }

                await Task.Delay(1000);
            });
        }

        public void Flush()
        {
            try
            {
                if (_client != null)
                {
                    _client.Flush();
                    System.Threading.Thread.Sleep(1000);

                    _client = null;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error tracking telemetry: {0}", ex.ToString());
            }
        }

        private bool RemoteKeyAvailable()
        {
            // Returns true if a valid AI key or tagged AI key exists
            bool validGuid = Guid.TryParse(_currentConfig.RemoteTelemetryKey, out var auxA);
            bool taggedGuid = Guid.TryParse(_currentConfig.RemoteTelemetryKey.Substring(4), out var auxB);
            return validGuid || taggedGuid;
        }

        private static string GetVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            return assembly.GetName().Version.ToString();
        }

        private static string GetFileVersion()
        {
            Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            return fvi.FileVersion;
        }

        ~TelemetryService()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                Flush();
            }

            // free native resources if any
        }
    }
}
