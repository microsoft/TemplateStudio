using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Diagnostics
{
    public class TelemetryEvents
    {
        public const string TemplateGenerated = TelemetryTracker.PropertiesPrefix + "TemplateGenerated";
    }

    public class TelemetryPages
    {
        public const string Project = TelemetryTracker.PropertiesPrefix + "Project";
        public const string Page = TelemetryTracker.PropertiesPrefix + "Page";
        public const string Feature = TelemetryTracker.PropertiesPrefix + "Feature";
    }

    public class TelemetryEventProperty
    {
        public const string BuildId = TelemetryTracker.PropertiesPrefix + "BuildId";
        public const string Name = TelemetryTracker.PropertiesPrefix + "Name";
        public const string Framework = TelemetryTracker.PropertiesPrefix + "Framework";
        public const string Type = TelemetryTracker.PropertiesPrefix + "Type";
    }
}
