using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Templates.Core.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core
{
    public class Diag
    {
        public EventTracker Verbose { get; private set; }
        public EventTracker Info { get; private set; }
        public EventTracker Warning { get; private set; }
        public EventTracker Error  { get; private set; }
        public ExceptionTracker Exceptions { get; private set; }
        public TelemetryTracker Telemetry { get; private set; }

        List<IDiagnosticsWriter> _listeners;

        static Diag _instance;
        public static Diag Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new Diag();
                }
                return _instance;
            }
        }

        private Diag()
        {
            _listeners = InstanceDefaultListeners();
            Verbose = new EventTracker(TraceEventType.Verbose, ref _listeners);
            Info = new EventTracker(TraceEventType.Information, ref _listeners);
            Warning = new EventTracker(TraceEventType.Warning, ref _listeners);
            Error = new EventTracker(TraceEventType.Error, ref _listeners);
            Exceptions = new ExceptionTracker(ref _listeners);
            Telemetry = new TelemetryTracker(ref _listeners);
        }

        public void AddListener(IDiagnosticsWriter newListener)
        {
            if(_listeners != null)
            {
                _listeners.Add(newListener);
            }
        }
        private List<IDiagnosticsWriter> InstanceDefaultListeners()
        {
            throw new NotImplementedException();
        }
    }
}
