using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Diagnostics
{
    public enum ActionStatusEnum
    {
        Completed = 0,
        Cancelled = 1,
        Error = 2 
    }
    public enum ActionEnum
    {
        Add = 0,
        Remove = 1
    }
    public class TelemetryEvents
    {
        public static string Project { get; private set; } = TelemetryTracker.PropertiesPrefix + "Project";
        public static string Page { get; private set; } = TelemetryTracker.PropertiesPrefix + "Page";
        public static string Wizard { get; private set; } = TelemetryTracker.PropertiesPrefix + "Wizard";
        public static string SessionStart { get; private set; } = TelemetryTracker.PropertiesPrefix + "SessionStart"; 
    }
    public class TelemetryProperties
    {
        public static string Action { get; private set; } = TelemetryTracker.PropertiesPrefix + "Action";
        public static string ActionStatus { get; private set; } = TelemetryTracker.PropertiesPrefix + "Status";
        public static string AppType { get; private set; } = TelemetryTracker.PropertiesPrefix + "AppType";
        public static string AppFx { get; private set; } = TelemetryTracker.PropertiesPrefix + "AppFx";
        public static string TemplateName { get; private set; } = TelemetryTracker.PropertiesPrefix + "TemplateName";
        public static string LastStep { get; private set; } = TelemetryTracker.PropertiesPrefix + "LastStep";
        public static string GenStatus { get; private set; } = TelemetryTracker.PropertiesPrefix + "GenStatus";
        public static string GenMessage { get; private set; } = TelemetryTracker.PropertiesPrefix + "GenMessage";
    }

    public class TelemetryMetrics
    {
        public static string PagesCount {get; private set;} = TelemetryTracker.PropertiesPrefix + "PagesCount";
        public static string TimeSpent { get; private set; } = TelemetryTracker.PropertiesPrefix + "TimeSpent";
        public static string FeaturesCount { get; private set; } = TelemetryTracker.PropertiesPrefix + "FeaturesCount";
    }
}
