using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Diagnostics
{
    public class TelemetryService : IDisposable
    {
        public Dictionary<string, string> Properties { get; } = new Dictionary<string, string>();
        public Dictionary<string, double> Metrics { get; } = new Dictionary<string, double>();
        public bool IsEnabled { get; private set; }

        private Configuration _currentConfig;
        private TelemetryClient _client { get; set; }

        public TelemetryService(Configuration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }
            _currentConfig = config;
            IntializeTelemetryClient();
        }

        public async Task TrackEventAsync(string eventName)
        {
            await SafeTrackAsync(() => _client.TrackEvent(eventName, Properties, Metrics)).ConfigureAwait(false);
        }

        public async Task TrackExceptionAsync(Exception ex)
        {
            await SafeTrackAsync(() => _client.TrackException(ex, Properties, Metrics)).ConfigureAwait(false);
        }

        private void IntializeTelemetryClient()
        {
            try
            {
                _client = new TelemetryClient()
                {
                    InstrumentationKey = _currentConfig.RemoteTelemetryKey
                };

                if (RemoteKeyAvailable())
                {
                    // Set session data
                    _client.Context.User.Id = Environment.UserName; //TODO: HASH USER NAME
                    _client.Context.Session.Id = Guid.NewGuid().ToString();
                    _client.Context.Device.OperatingSystem = Environment.OSVersion.ToString();
                    _client.Context.Component.Version = GetVersion();

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


        private async Task SafeTrackAsync(Action trackAction)
        {
            try
            {
                var task = Task.Run(() => {
                    trackAction();
                    Properties.Clear();
                    Metrics.Clear();
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
        private bool RemoteKeyAvailable()
        {
            return Guid.TryParse(_currentConfig.RemoteTelemetryKey, out var aux);
        }

        private static string GetVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            return assembly.GetName().Version.ToString();
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
                if (_client != null)
                {
                    _client.Flush();
                }
            }
            //free native resources if any.
        }


    }
}
