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

namespace Microsoft.Templates.Core.Diagnostics
{
    public class TelemetryProperties
    {
        public static string WizardFileVersion { get; internal set; } = TelemetryTracker.PropertiesPrefix + "WizardFileVersion";
        public static string WizardContentVersion { get; internal set; } = TelemetryTracker.PropertiesPrefix + "TemplatesVersion";
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
        public static string VisualStudioVersion { get; internal set; } = TelemetryTracker.PropertiesPrefix + "VsVersion";
        public static string VisualStudioEdition { get; internal set; } = TelemetryTracker.PropertiesPrefix + "VsEdition";
        public static string VisualStudioCulture { get; internal set; } = TelemetryTracker.PropertiesPrefix + "VsCulture";
        public static string SummaryItemEditAction { get; internal set; } = TelemetryTracker.PropertiesPrefix + "ItemEditAction";
    }
}
