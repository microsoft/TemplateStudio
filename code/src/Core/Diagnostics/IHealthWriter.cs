using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Diagnostics
{
    public interface IHealthWriter
    {
        Task WriteTraceAsync(TraceEventType eventType, string message, Exception ex=null);
        Task WriteExceptionAsync(Exception ex, string message = null);
        bool AllowMultipleInstances();
    }
}