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
        public static string WizardFileVersion { get; private set; } = TelemetryEvents.Prefix + "WizardFileVersion";
        public static string WizardContentVersion { get; private set; } = TelemetryEvents.Prefix + "TemplatesVersion";
        public static string RoleInstanceName { get; private set; } = TelemetryEvents.Prefix + "WizardClient";
        public static string Status { get; private set; } = TelemetryEvents.Prefix + "Status";
        public static string ProjectType { get; private set; } = TelemetryEvents.Prefix + "ProjectType";
        public static string Framework { get; private set; } = TelemetryEvents.Prefix + "Framework";
        public static string TemplateName { get; private set; } = TelemetryEvents.Prefix + "TemplateName";
        public static string GenEngineStatus { get; private set; } = TelemetryEvents.Prefix + "GenEngineStatus";
        public static string GenEngineMessage { get; private set; } = TelemetryEvents.Prefix + "GenEngineMessage";
        public static string WizardType { get; private set; } = TelemetryEvents.Prefix + "WizardType";
        public static string WizardStatus { get; private set; } = TelemetryEvents.Prefix + "WizardStatus";
        public static string LastStep { get; private set; } = TelemetryEvents.Prefix + "LastStep";
        public static string EventName { get; private set; } = TelemetryEvents.Prefix + "EventName";
        public static string VisualStudioVersion { get; private set; } = TelemetryEvents.Prefix + "VsVersion";
        public static string VisualStudioEdition { get; private set; } = TelemetryEvents.Prefix + "VsEdition";
        public static string VisualStudioCulture { get; private set; } = TelemetryEvents.Prefix + "VsCulture";
        public static string VisualStudioManifestId { get; private set; } = TelemetryEvents.Prefix + "VsManifestId";
        public static string VisualStudioActiveProjectGuid { get; private set; } = TelemetryEvents.Prefix + "VsActiveProjectGuid";
        public static string SummaryItemEditAction { get; private set; } = TelemetryEvents.Prefix + "ItemEditAction";
        public static string VsProjectCategory { get; private set; } = TelemetryEvents.Prefix + "Category";
    }
}
