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
        public bool IsEnabled { get; private set; }

        private Configuration _currentConfig;
        private TelemetryClient _client { get; set; }

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
            await SafeTrackAsync(() => _client.TrackEvent(eventName, properties, metrics)).ConfigureAwait(false);
        }

        public async Task TrackExceptionAsync(Exception ex, Dictionary<string, string> properties = null, Dictionary<string, double> metrics = null)
        {
            await SafeTrackAsync(() => _client.TrackException(ex, properties, metrics)).ConfigureAwait(false);
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
                    _client.Context.User.Id = Environment.UserName; //TODO: HASH USER NAME??
                    _client.Context.Session.Id = Guid.NewGuid().ToString();
                    _client.Context.Device.OperatingSystem = Environment.OSVersion.ToString();
                    _client.Context.Component.Version = GetVersion();

                    _client.TrackEvent(TelemetryEvents.SessionStarted);

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
            if (_client != null)
            {
                _client.TrackEvent(TelemetryEvents.SessionEnded);
                _client.Flush();
                _client = null;
            }
            await Task.Delay(1000);
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
                    System.Threading.Thread.Sleep(1000);

                    _client = null;
                }
            }
            //free native resources if any.
        }
    }
}
