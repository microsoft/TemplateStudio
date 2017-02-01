using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Diagnostics
{
    public enum ActionStatus
    {
        Completed,
        Cancelled,
        Error
    }

    public class TelemetryEvents
    {
        public static string NewProject { get; private set; } = TelemetryTracker.PropertiesPrefix + "NewProject";
        public static string NewPage { get; private set; } = TelemetryTracker.PropertiesPrefix + "NewPage";
        public static string Wizard { get; private set; } = TelemetryTracker.PropertiesPrefix + "Wizard";
        public static string SessionStart { get; private set; } = TelemetryTracker.PropertiesPrefix + "SessionStart";
    }
    public class TelemetryProperties
    {
        public static string ActionStatus { get; private set; } = TelemetryTracker.PropertiesPrefix + "Status";
        public static string AppType { get; private set; } = TelemetryTracker.PropertiesPrefix + "AppType";
        public static string AppFx { get; private set; } = TelemetryTracker.PropertiesPrefix + "AppFx";
        public static string TemplateName { get; private set; } = TelemetryTracker.PropertiesPrefix + "TemplateName";
        public static string LastStep { get; private set; } = TelemetryTracker.PropertiesPrefix + "LastStep";
    }

    public class TelemetryMetrics
    {
        public static string PagesCount {get; private set;} = TelemetryTracker.PropertiesPrefix + "PagesCount";
        public static string TimeSpent { get; private set; } = TelemetryTracker.PropertiesPrefix + "TimeSpent";
        public static string FeaturesAddedCount { get; private set; } = TelemetryTracker.PropertiesPrefix + "FeaturesAddedCount";
        public static string FeaturesRemovedCount { get; private set; } = TelemetryTracker.PropertiesPrefix + "FeaturesRemovedCount";
        public static string FeaturesDefaultCount { get; private set; } = TelemetryTracker.PropertiesPrefix + "FeaturesDefaultCount";
    }
}
