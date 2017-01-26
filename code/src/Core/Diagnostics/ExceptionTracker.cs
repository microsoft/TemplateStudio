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
        public ExceptionTracker()
        {
        }

        public async Task TrackAsync(Exception ex, string message=null)
        {
            try
            {
                foreach (IHealthWriter writer in HealthWriters.Available)
                {
                    await writer.WriteExceptionAsync(ex, message).ConfigureAwait(false);
                }
            }
            catch (Exception exception)
            {
                Trace.TraceError($"Error writing exception data to listeners. Exception:\r\n{exception.ToString()}");
            }
        }
    }
}
