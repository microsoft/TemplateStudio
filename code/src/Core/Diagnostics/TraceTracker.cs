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
    public class TraceTracker
    {
        private TraceEventType _traceEventType;

        public TraceTracker(TraceEventType eventType)
        {
            _traceEventType = eventType;
        }

        public async Task TrackAsync(string message, Exception ex = null)
        {
            if (IsTraceEnabled())
            {
                foreach (IHealthWriter writer in HealthWriters.Available)
                {
                    await SafeTrackAsync(message, ex, writer);
                }
            }
        }

        private async Task SafeTrackAsync(string message, Exception ex, IHealthWriter writer)
        {
            try
            {
                await writer.WriteTraceAsync(_traceEventType, message, ex).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                Trace.TraceError($"Error writing event to listener {writer.GetType().ToString()}. Exception:\r\n{exception.ToString()}");
            }
        }

        private bool IsTraceEnabled()
        {
            return _traceEventType <= Configuration.Current.DiagnosticsTraceLevel;
        }
    }
}
