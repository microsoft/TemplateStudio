// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

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
        public static string FeatureGen { get; private set; } = TelemetryTracker.PropertiesPrefix + "FeatureGen";
        public static string Wizard { get; private set; } = TelemetryTracker.PropertiesPrefix + "Wizard";
        public static string SessionStart { get; private set; } = TelemetryTracker.PropertiesPrefix + "SessionStart"; 
    }
    public class TelemetryProperties
    {
        public static string WizardFileVersion { get; internal set; } = TelemetryTracker.PropertiesPrefix + "WizardFileVersion"; 
        public static string WizardContentVersion { get; internal set; } = TelemetryTracker.PropertiesPrefix + "WizardContentVersion";
        public static string RoleInstanceName { get; internal set; } = TelemetryTracker.PropertiesPrefix + "WizardClient";
        public static string Status { get; private set; } = TelemetryTracker.PropertiesPrefix + "Status";
        public static string ProjectType { get; private set; } = TelemetryTracker.PropertiesPrefix + "ProjectType";
        public static string Framework { get; private set; } = TelemetryTracker.PropertiesPrefix + "Framework";
        public static string TemplateName { get; private set; } = TelemetryTracker.PropertiesPrefix + "TemplateName";
        public static string GenEngineStatus { get; private set; } = TelemetryTracker.PropertiesPrefix + "GenEngineStatus";
        public static string GenEngineMessage { get; private set; } = TelemetryTracker.PropertiesPrefix + "GenEngineMessage";
        public static string WizardType { get; private set; } = TelemetryTracker.PropertiesPrefix + "WizardType";
        public static string WizardStatus { get; private set; } = TelemetryTracker.PropertiesPrefix + "WizardStatus";
        public static string LastStep { get; private set; } = TelemetryTracker.PropertiesPrefix + "LastStep";
        public static string EventName { get; internal set; } = TelemetryTracker.PropertiesPrefix + "EventName";
    }

    public class TelemetryMetrics
    {
        public static string PagesCount {get; private set;} = TelemetryTracker.PropertiesPrefix + "PagesCount";
        public static string TimeSpent { get; private set; } = TelemetryTracker.PropertiesPrefix + "TimeSpent";
        public static string FeaturesCount { get; private set; } = TelemetryTracker.PropertiesPrefix + "FeaturesCount";
    }
}
