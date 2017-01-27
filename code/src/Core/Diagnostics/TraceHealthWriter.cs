using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Diagnostics
{
    public class TraceHealthWriter : IHealthWriter
    {
        public async Task WriteTraceAsync(TraceEventType eventType, string message, Exception ex = null)
        {

            string formattedMessage = FormattedWriterMessages.LogEntryStart + $"\t{eventType.ToString()}\t{message}";
            if(ex != null)
            {
                formattedMessage = formattedMessage + $"\tException:\n\r{ex.ToString()}";
            }

            switch (eventType)
            {
                case TraceEventType.Critical:
                case TraceEventType.Error:
                    await CallAsync(() => Trace.TraceError(formattedMessage));
                    break;
                case TraceEventType.Warning:
                    await CallAsync(() => Trace.TraceWarning(formattedMessage));
                    break;
                case TraceEventType.Information:
                case TraceEventType.Verbose:
                    await CallAsync(() => Trace.TraceInformation(formattedMessage));
                    break;
                default:
                    await CallAsync(() => Trace.TraceInformation(formattedMessage));
                    break;
            }

            Debug.WriteLine(formattedMessage);
        }

        public async Task WriteExceptionAsync(Exception ex, string message = null)
        {
            await WriteTraceAsync(TraceEventType.Critical, "Exception Tracked", ex);
        }

        private async Task CallAsync(Action action)
        {
            var task = Task.Run(() => action);
            await task;
        }
    }
}
