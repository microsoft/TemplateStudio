using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Diagnostics
{
    public class DiagnosticWriter
    {
        TraceLevel _level;
        List<IDiagnosticListener> _listeners;
        public DiagnosticWriter(TraceLevel level, List<IDiagnosticListener> listeners)
        {
            _level = level;
            _listeners = listeners;
        }

        public void Send(string message, Exception ex)
        {
            try
            {
                foreach (IDiagnosticListener listener in _listeners)
                {
                    listener.WriteDataAsync(_level, message, ex);
                }
            }
            catch (Exception exception)
            {
                Debug.Write($"Error writing data to listeners. Exception:\r\n{exception.ToString()}");
            }
        }

        public void Send(string message)
        {
            Send(message, null);
        }
    }
}
