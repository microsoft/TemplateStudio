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
        public TelemetryTracker()
        {
        }
        public TelemetryTracker(Configuration config)
        { 
            TelemetryService.SetConfiguration(config);
        }
        public async Task TrackTemplateGeneratedAsync(string name, string framework, string type)
        {
            Dictionary<string, string> properties = new Dictionary<string, string>()
            {
                {TelemetryEventProperty.Name, "EventName" },
                {TelemetryEventProperty.Framework, "Caliburn" },
                { TelemetryEventProperty.Type, "Page" }
            };
            await TelemetryService.Current.TrackEventAsync(TelemetryEvents.TemplateGenerated, properties).ConfigureAwait(false);
        }
    }
}
