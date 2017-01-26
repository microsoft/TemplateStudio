using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Diagnostics
{
    public class AppHealth
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
                    _current = new AppHealth(Configuration.Current);
                }
                return _current;
            }
        }

        public AppHealth(Configuration currentConfig)
        {
            InstanceDefaultWriters(currentConfig);
            Verbose = new TraceTracker(TraceEventType.Verbose, currentConfig.DiagnosticsTraceLevel);
            Info = new TraceTracker(TraceEventType.Information, currentConfig.DiagnosticsTraceLevel);
            Warning = new TraceTracker(TraceEventType.Warning, currentConfig.DiagnosticsTraceLevel);
            Error = new TraceTracker(TraceEventType.Error, currentConfig.DiagnosticsTraceLevel);
            Exception = new ExceptionTracker();
            Telemetry = new TelemetryTracker();
            _current = this;
        }

        public void AddWriter(IHealthWriter newWriter)
        {
            if (!HealthWriters.Available.Where(w => w.GetType() == newWriter.GetType()).Any())
            {
                HealthWriters.Available.Add(newWriter);
            }
        }
        private void InstanceDefaultWriters(Configuration currentConfig)
        {
            HealthWriters.Available.Add(new FileHealthWriter(currentConfig));
            HealthWriters.Available.Add(new TraceHealthWriter());
            HealthWriters.Available.Add(new RemoteHealthWriter(currentConfig));
        }
    }
}
