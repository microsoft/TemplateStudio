using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Diagnostics
{
    public interface IHealthWriter
    {
        //TODO: Convert to an abstract class. Consider intialization.
        Task WriteTraceAsync(TraceEventType eventType, string message, Exception ex=null);
        Task WriteExceptionAsync(Exception ex, string message = null);
        bool AllowMultipleInstances();
    }
}