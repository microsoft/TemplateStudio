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

        private TelemetryService _telemetry;
        private Configuration _currentConfig;

        public TelemetryTracker() : this(Configuration.Default)
        {
        }
        public TelemetryTracker(Configuration config)
        {
            _currentConfig = config;
            _telemetry = new TelemetryService(_currentConfig);
        }
        public async Task TrackTemplateGeneratedAsync(string name, string framework, string type)
        {
            _telemetry.Properties.Add(TelemetryEventProperty.Name, "OtherData");
            _telemetry.Properties.Add(TelemetryEventProperty.Framework, "Caliburn");
            _telemetry.Properties.Add(TelemetryEventProperty.Type, "Page");
            await _telemetry.TrackEventAsync(TelemetryEvents.TemplateGenerated).ConfigureAwait(false);
        }
    }
}
