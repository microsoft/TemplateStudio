using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Diagnostics
{
    public class TraceTracker
    {
        private TraceEventType _traceEventType;
        private bool _traceEnabled;

        public TraceTracker(TraceEventType eventType, TraceEventType traceLevel)
        {
            _traceEventType = eventType;
            _traceEnabled = traceLevel >= eventType;
        }

        public async Task TrackAsync(string message, Exception ex=null)
        {
            if (!_traceEnabled)
            {
                return;
            }

            foreach (IHealthWriter writer in HealthWriters.Available)
            {
                await SafeTrackAsync(message, ex, writer);
            }
        }

        private async Task SafeTrackAsync(string message, Exception ex, IHealthWriter writer)
        {
            try
            {
                await writer.WriteTraceAsync(_traceEventType, message, ex).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                Trace.TraceError($"Error writing event to listener {writer.GetType().ToString()}. Exception:\r\n{exception.ToString()}");
            }
        }
    }
}
