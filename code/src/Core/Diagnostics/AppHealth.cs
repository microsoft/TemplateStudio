// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Linq;

using Microsoft.Templates.Core.Gen;

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

        private static AppHealth _current;

        public static AppHealth Current
        {
            get
            {
                if (_current == null)
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
            if (newWriter != null)
            {
                if (newWriter.AllowMultipleInstances() || !HealthWriters.Available.Where(w => w.GetType() == newWriter.GetType()).Any())
                {
                    HealthWriters.Available.Add(newWriter);
                }
            }
        }

        public void IntializeTelemetryClient(GenShell shell)
        {
            TelemetryService.Current.IntializeTelemetryClient(shell);
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
                foreach (IHealthWriter writer in HealthWriters.Available)
                {
                    if (writer is IDisposable disposableWriter)
                    {
                        disposableWriter.Dispose();
                    }
                }
            }

            // free native resources if any.
        }
    }
}
