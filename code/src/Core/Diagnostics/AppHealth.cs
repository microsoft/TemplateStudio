using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Verbose = new TraceTracker(TraceEventType.Verbose, Configuration.Current.DiagnosticsTraceLevel);
            Info = new TraceTracker(TraceEventType.Information, Configuration.Current.DiagnosticsTraceLevel);
            Warning = new TraceTracker(TraceEventType.Warning, Configuration.Current.DiagnosticsTraceLevel);
            Error = new TraceTracker(TraceEventType.Error, Configuration.Current.DiagnosticsTraceLevel);
            Exception = new ExceptionTracker();
            Telemetry = new TelemetryTracker();
        }

        public void Restart()
        {
            _current = new AppHealth();
        }

        public void AddWriter(IHealthWriter newWriter)
        {
            HealthWriters.Available.Add(newWriter);
        }
        private void InstanceDefaultWriters()
        {
            HealthWriters.Available.Add(FileHealthWriter.Current);
            HealthWriters.Available.Add(new TraceHealthWriter());
            HealthWriters.Available.Add(RemoteHealthWriter.Current);
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
                    IDisposable disposableWriter = writer as IDisposable;
                    if(disposableWriter != null)
                    {
                        disposableWriter.Dispose();
                    }
                }
            }
            //free native resources if any.
        }
    }
}
