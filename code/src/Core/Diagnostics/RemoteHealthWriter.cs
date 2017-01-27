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
    public class RemoteHealthWriter : IHealthWriter
    {

        public RemoteHealthWriter(Configuration currentConfig)
        {
            TelemetryService.SetConfiguration(currentConfig);
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
    }
}
