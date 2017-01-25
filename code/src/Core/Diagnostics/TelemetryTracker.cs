using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Diagnostics
{
    public class TelemetryTracker
    {
        public const string PropertiesPrefix = "UCT ";
        public Dictionary<string, string> Properties { get; } = new Dictionary<string, string>();
        public Dictionary<string, double> Metrics { get; } = new Dictionary<string, double>();

        private List<IDiagnosticsWriter> _listeners;
        public TelemetryTracker(ref List<IDiagnosticsWriter> listeners)
        {
            _listeners = listeners;
        }
    }
}
