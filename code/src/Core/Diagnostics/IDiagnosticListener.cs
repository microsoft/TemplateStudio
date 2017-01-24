using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Diagnostics
{
    public interface IDiagnosticListener
    {
        Task WriteDataAsync(TraceLevel level, string message, Exception ex);
    }
}