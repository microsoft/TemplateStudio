// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace Microsoft.Templates.Core.Diagnostics
{
    public class TelemetryService : IHealthWriter, IDisposable
    {
        public bool IsEnabled { get; private set; }

        private Configuration _currentConfig;
        private TelemetryClient _client;

        public static TelemetryService _current;
        public static TelemetryService Current
        {
            get
            {
                if(_current == null)
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
            _currentConfig = config ?? throw new ArgumentNullException("config");

            IntializeTelemetryClient();
        }

        public static void SetConfiguration(Configuration config)
        {
            _current = new TelemetryService(config);
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
            //Trace events will not be forwarded to the remote service
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

        private void IntializeTelemetryClient()
        {
            try
            {
                _client = new TelemetryClient()
                {
                    InstrumentationKey = _currentConfig.RemoteTelemetryKey
                };

                if (VsTelemetryIsOptedIn() && RemoteKeyAvailable())
                {
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

        private bool VsTelemetryIsOptedIn()
        {
            try
            {
                Assembly.Load(new AssemblyName("Microsoft.VisualStudio.Telemetry"));
                return SafeVsTelemetryIsOptedIn();
            }
            catch(Exception ex)
            {
                //Not running in VS so we assume we are in the emulator => we allow telemetry
                Trace.TraceWarning($"Unable to load the assembly 'Microsoft.VisualStudio.Telemetry'. Visual Studio Telemetry OptIn/OptOut setting will not be considered. Details:\r\n{ex.ToString()}");
                return true;
            }
        }
        
        private bool SafeVsTelemetryIsOptedIn()
        {
            try
            {
                if (Microsoft.VisualStudio.Telemetry.TelemetryService.DefaultSession != null)
                {
                    var isOptedIn = Microsoft.VisualStudio.Telemetry.TelemetryService.DefaultSession.IsOptedIn;
                    Trace.TraceInformation($"Vs Telemetry IsOptedIn: {isOptedIn}");
                    return isOptedIn;
                }
                else
                {
                    //Not running in VS so we assume we are in the emulator => we allow telemetry
                    Trace.TraceInformation($"Checking VsTelemetry IsOptedIn value Microsoft.VisualStudio.Telemetry.TelemetryService.DefaultSession is Null.");
                    return true;
                }
            }
            catch(Exception ex)
            {
                //Not running in VS so we assume we are in the emulator => we allow telemetry
                Trace.TraceInformation($"Exception checking VsTelemetry IsOptedIn:\r\n" + ex.ToString());
                return true;
            }
        }

        private void SetSessionData()
        {
            string userToTrack =  Guid.NewGuid().ToString();
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

        public void SetContentVersionToContext(Version wizardContentVersion)
        {
            if (wizardContentVersion != null && _client != null && _client.Context != null && _client.Context.Properties != null)
            {
                if (!_client.Context.Properties.ContainsKey(TelemetryProperties.WizardContentVersion))
                {
                    _client.Context.Properties.Add(TelemetryProperties.WizardContentVersion, wizardContentVersion.ToString());
                }
                else
                {
                    _client.Context.Properties[TelemetryProperties.WizardContentVersion] = wizardContentVersion.ToString();
                }
            }
        }

        public void SetVisualStudioInfoToContext(string version, string edition, string culture)
        {
            if (_client != null && _client.Context != null && _client.Context.Properties != null)
            {
                if (!string.IsNullOrEmpty(version))
                {
                    _client.Context.Properties[TelemetryProperties.VisualStudioVersion] = version;
                }
                if (!string.IsNullOrEmpty(edition))
                {
                    _client.Context.Properties[TelemetryProperties.VisualStudioEdition] = edition;
                }
                if (!string.IsNullOrEmpty(culture))
                {
                    _client.Context.Properties[TelemetryProperties.VisualStudioCulture] = culture;
                }
            }
        }

            private async Task SafeExecuteAsync(Action action)
        {
            try
            {
                var task = Task.Run(() => {
                    action();
                });

                await task;
            }
            catch (AggregateException aggEx)
            {
                foreach (Exception ex in aggEx.InnerExceptions)
                {
                    Trace.TraceError("Error tracking telemetry: {0}", ex.ToString());
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error tracking telemetry: {0}", ex.ToString());
            }
        }

        public async Task FlushAsync()
        {
            await SafeExecuteAsync( async () => {
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
            return Guid.TryParse(_currentConfig.RemoteTelemetryKey, out var aux);
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
            //free native resources if any.
        }
    }
}
