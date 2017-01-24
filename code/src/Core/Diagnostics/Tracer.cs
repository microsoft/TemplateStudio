using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Diagnostics
{
    public class Tracer
    {
        public DiagnosticWriter Verbose { get; private set; }
        public DiagnosticWriter Info { get; private set; }
        public DiagnosticWriter Warning { get; private set; }
        public DiagnosticWriter Error  { get; private set; }

        List<IDiagnosticListener> _listeners;

        static Tracer _instance;
        public static Tracer Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new Tracer();
                }
                return _instance;
            }
        }

        private Tracer()
        {
            _listeners = InstanceAvailableListeners();
            Verbose = new DiagnosticWriter(TraceLevel.Verbose, _listeners);
            Info = new DiagnosticWriter(TraceLevel.Info, _listeners);
            Warning = new DiagnosticWriter(TraceLevel.Warning, _listeners);
            Error = new DiagnosticWriter(TraceLevel.Error, _listeners);
        }

        public void AddListener(IDiagnosticListener newListener)
        {
            if(_listeners != null)
            {
                _listeners.Add(newListener);
            }
        }

        private List<IDiagnosticListener> InstanceAvailableListeners()
        {
            throw new NotImplementedException();
        }
    }
}
