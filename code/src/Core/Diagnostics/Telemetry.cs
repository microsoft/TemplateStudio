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
    public class Telemetry : IDisposable
    {
        private TelemetryClient Client {get; set;}
        public Dictionary<string, string> Properties { get; } = new Dictionary<string, string>();
        public Dictionary<string, double> Metrics { get; } = new Dictionary<string, double>();

        public bool IsEnabled { get; private set; }

        private Configuration _currentConfig;

        public Telemetry(Configuration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }
            _currentConfig = config;
            IntializeTelemetryClient();
        }
        private void IntializeTelemetryClient()
        {
            try
            {
                if (RemoteKeyAvailable())
                {
                    Client = new TelemetryClient()
                    {
                        InstrumentationKey = _currentConfig.RemoteTelemetryKey
                    };

                    // Set session data
                    Client.Context.User.Id = Environment.UserName; //TODO: HASH USER NAME
                    Client.Context.Session.Id = Guid.NewGuid().ToString();
                    Client.Context.Device.OperatingSystem = Environment.OSVersion.ToString();
                    Client.Context.Component.Version = GetVersion();

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

        public void TrackEvent(string eventName)
        {
            SafeTrack(()=>Client.TrackEvent(eventName, Properties, Metrics));
        }

        public void TrackException(Exception ex)
        {
            SafeTrack(() => Client.TrackException(ex, Properties, Metrics));
        }
        private void SafeTrack(Action trackAction)
        {
            try
            {
                trackAction();
                Properties.Clear();
                Metrics.Clear();
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

        ~Telemetry()
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
                if(Client != null)
                {
                    Client.Flush();
                    System.Threading.Thread.Sleep(1000); //TODO:??
                }
            }
            //free native resources if any.
        }


    }
}
