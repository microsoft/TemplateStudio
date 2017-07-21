// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Diagnostics
{
    public class ExceptionTracker
    {
        public ExceptionTracker()
        {
        }

        public async Task TrackAsync(Exception ex, string message = null)
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
