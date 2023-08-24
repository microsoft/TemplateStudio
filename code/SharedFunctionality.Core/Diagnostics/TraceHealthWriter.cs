// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Templates.SharedResources;

namespace Microsoft.Templates.Core.Diagnostics
{
    public class TraceHealthWriter : IHealthWriter
    {
        public async Task WriteTraceAsync(TraceEventType eventType, string message, Exception ex = null)
        {
            string formattedMessage = FormattedWriterMessages.LogEntryStart + $"\t{eventType.ToString()}\t{message}";
            if (ex != null)
            {
                formattedMessage = formattedMessage + $"\t{Resources.ExceptionString}:\n\r{ex.ToString()}";
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
            await WriteTraceAsync(TraceEventType.Critical, Resources.ExceptionTrackedString, ex);
        }

        private async Task CallAsync(Action action)
        {
            var task = Task.Run(() => action);

            await task;
        }

        public bool AllowMultipleInstances()
        {
            return false;
        }
    }
}
