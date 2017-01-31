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

        public TraceTracker(TraceEventType eventType)
        {
            _traceEventType = eventType;
        }

        public async Task TrackAsync(string message, Exception ex=null)
        {
            if (IsTraceEnabled())
            {
                foreach (IHealthWriter writer in HealthWriters.Available)
                {
                    await SafeTrackAsync(message, ex, writer);
                }
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

        private bool IsTraceEnabled()
        {
            return _traceEventType <= Configuration.Current.DiagnosticsTraceLevel;
        }
    }
}
