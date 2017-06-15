// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Diagnostics
{
    public class TraceHealthWriter : IHealthWriter
    {
        public async Task WriteTraceAsync(TraceEventType eventType, string message, Exception ex = null)
        {
            string formattedMessage = FormattedWriterMessages.LogEntryStart + $"\t{eventType.ToString()}\t{message}";
            if (ex != null)
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

        public bool AllowMultipleInstances()
        {
            return false;
        }
    }
}
