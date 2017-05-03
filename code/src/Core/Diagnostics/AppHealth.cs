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
using System.Linq;

namespace Microsoft.Templates.Core.Diagnostics
{
    public class AppHealth : IDisposable
    {
        public TraceTracker Verbose { get; private set; }
        public TraceTracker Info { get; private set; }
        public TraceTracker Warning { get; private set; }
        public TraceTracker Error { get; private set; }
        public ExceptionTracker Exception { get; private set; }
        public TelemetryTracker Telemetry { get; private set; }

        static AppHealth _current;
        public static AppHealth Current
        {
            get
            {
                if(_current == null)
                {
                    _current = new AppHealth();
                }

                return _current;
            }

            private set
            {
                _current = value;
            }
        }


        private AppHealth()
        {
            InstanceDefaultWriters();

            Verbose = new TraceTracker(TraceEventType.Verbose);
            Info = new TraceTracker(TraceEventType.Information);
            Warning = new TraceTracker(TraceEventType.Warning);
            Error = new TraceTracker(TraceEventType.Error);
            Exception = new ExceptionTracker();
            Telemetry = new TelemetryTracker();
        }

        public void AddWriter(IHealthWriter newWriter)
        {
            if (newWriter.AllowMultipleInstances() || !HealthWriters.Available.Where(w => w.GetType() == newWriter.GetType()).Any())
            {
                HealthWriters.Available.Add(newWriter);
            }
        }
        private void InstanceDefaultWriters()
        {
            HealthWriters.Available.Add(FileHealthWriter.Current);
            HealthWriters.Available.Add(new TraceHealthWriter());
            HealthWriters.Available.Add(TelemetryService.Current);
        }

        ~AppHealth()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources 
                foreach(IHealthWriter writer in HealthWriters.Available)
                {
                    if (writer is IDisposable disposableWriter)
                    {
                        disposableWriter.Dispose();
                    }
                }
            }
            //free native resources if any.
        }
    }
}
