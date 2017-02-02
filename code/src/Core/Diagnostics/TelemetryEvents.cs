using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Diagnostics
{
    public enum GenStatusEnum
    {
        Completed = 0,
        Cancelled = 1,
        Error = 2 
    }

    public enum WizardStatusEnum
    {
        Completed = 0,
        Cancelled = 1,
        Error
    }
    public enum WizardTypeEnum
    {
        NewProject = 0,
        AddPage = 1,
        AddFeature
    }

    public class TelemetryEvents
    {
        public static string ProjectGen { get; private set; } = TelemetryTracker.PropertiesPrefix + "ProjectGen";
        public static string PageGen { get; private set; } = TelemetryTracker.PropertiesPrefix + "PageGen";
        public static string Wizard { get; private set; } = TelemetryTracker.PropertiesPrefix + "Wizard";
        public static string SessionStart { get; private set; } = TelemetryTracker.PropertiesPrefix + "SessionStart"; 
    }
    public class TelemetryProperties
    {
        public static string Status { get; private set; } = TelemetryTracker.PropertiesPrefix + "Status";
        public static string AppType { get; private set; } = TelemetryTracker.PropertiesPrefix + "AppType";
        public static string AppFx { get; private set; } = TelemetryTracker.PropertiesPrefix + "AppFx";
        public static string TemplateName { get; private set; } = TelemetryTracker.PropertiesPrefix + "TemplateName";
        public static string LastStep { get; private set; } = TelemetryTracker.PropertiesPrefix + "LastStep";
        public static string GenEngineStatus { get; private set; } = TelemetryTracker.PropertiesPrefix + "GenEngineStatus";
        public static string GenEngineMessage { get; private set; } = TelemetryTracker.PropertiesPrefix + "GenEngineMessage";
        public static string WizardType { get; private set; } = TelemetryTracker.PropertiesPrefix + "WizardType";
        public static string WizardStatus { get; private set; } = TelemetryTracker.PropertiesPrefix + "WizardStatus";
    }

    public class TelemetryMetrics
    {
        public static string PagesCount {get; private set;} = TelemetryTracker.PropertiesPrefix + "PagesCount";
        public static string TimeSpent { get; private set; } = TelemetryTracker.PropertiesPrefix + "TimeSpent";
        public static string FeaturesCount { get; private set; } = TelemetryTracker.PropertiesPrefix + "FeaturesCount";
    }
}
