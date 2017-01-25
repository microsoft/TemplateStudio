using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Diagnostics
{
    public class EventTracker
    {
        private TraceEventType _traceEventType;
        private List<IDiagnosticsWriter> _listeners;
        public EventTracker(TraceEventType eventType, ref List<IDiagnosticsWriter> listeners)
        {
            _traceEventType = eventType;
            _listeners = listeners;
        }

        public void Track(string message, Exception ex)
        {
            try
            {
                foreach (IDiagnosticsWriter listener in _listeners)
                {
                    listener.WriteEventAsync(_traceEventType, message, ex);
                }
            }
            catch (Exception exception)
            {
                Debug.Write($"Error writing event to listeners. Exception:\r\n{exception.ToString()}");
            }
        }
    }
}
