using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Diagnostics
{
    public class ExceptionTracker
    {
        private List<IDiagnosticsWriter> _listeners;
        public ExceptionTracker(ref List<IDiagnosticsWriter> listeners)
        {
            _listeners = listeners;
        }

        public void Track(Exception ex, string message=null)
        {
            try
            {
                foreach (IDiagnosticsWriter listener in _listeners)
                {
                    listener.WriteExceptionAsync(ex, message);
                }
            }
            catch (Exception exception)
            {
                Trace.TraceError($"Error writing exception data to listeners. Exception:\r\n{exception.ToString()}");
            }
        }
    }
}
