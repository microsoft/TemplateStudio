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

        TelemetryService _telemetry;
        public RemoteHealthWriter(Configuration currentConfig)
        {
            _telemetry = new TelemetryService(currentConfig);
        }
        public async Task WriteTraceAsync(TraceEventType eventType, string message, Exception ex=null)
        {
            //Trace events will not be forwarded to the remote service
            return; 
        }

        public async Task WriteExceptionAsync(Exception ex, string message = null)
        {
            await _telemetry.TrackExceptionAsync(ex);
        }
    }
}
