using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Diagnostics
{
    public class RemoteHealthWriter : IHealthWriter, IDisposable
    {
        //TODO, Review
        private static RemoteHealthWriter _current;
        public static RemoteHealthWriter Current
        {
            get
            {
                if (_current == null)
                {
                    _current = new RemoteHealthWriter(Configuration.Current);
                }
                return _current;
            }
        }

        private RemoteHealthWriter(Configuration currentConfig)
        {
            TelemetryService.SetConfiguration(currentConfig);
        }

        public static void SetConfiguration(Configuration config)
        {
            _current = new RemoteHealthWriter(config);
        }

        public async Task WriteTraceAsync(TraceEventType eventType, string message, Exception ex=null)
        {
            //Trace events will not be forwarded to the remote service
            await Task.Run(() => { });
        }

        public async Task WriteExceptionAsync(Exception ex, string message = null)
        {
            await TelemetryService.Current.TrackExceptionAsync(ex);
        }

        ~RemoteHealthWriter()
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
                TelemetryService.Current.Dispose();
            }
            //free native resources if any.
        }
    }
}
