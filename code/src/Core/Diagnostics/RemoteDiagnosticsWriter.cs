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
    public class RemoteDiagnosticsWriter : IDiagnosticsWriter
    {

        Telemetry telemetry = new Telemetry(Configuration.Current);
        public RemoteDiagnosticsWriter()
        {
        } 
        public async Task WriteEventAsync(TraceEventType eventType, string message, Exception ex=null)
        {
            //Event will not be forwarded to the remote service
            return; 
        }

        public async Task WriteExceptionAsync(Exception ex, string message = null)
        {
            await CallAsync(() => telemetry.Client.TrackException(ex));
        }

        private async Task CallAsync(Action action)
        {
            var task = Task.Run(() => action);
            await task;
        }
    }
}
