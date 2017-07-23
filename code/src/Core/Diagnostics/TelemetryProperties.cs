// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
        public static string WizardAction { get; private set; } = TelemetryEvents.Prefix + "WizardAction";
        public static string LastStep { get; private set; } = TelemetryEvents.Prefix + "LastStep";
        public static string EventName { get; private set; } = TelemetryEvents.Prefix + "EventName";
        public static string VisualStudioVersion { get; private set; } = TelemetryEvents.Prefix + "VsVersion";
        public static string VisualStudioEdition { get; private set; } = TelemetryEvents.Prefix + "VsEdition";
        public static string VisualStudioCulture { get; private set; } = TelemetryEvents.Prefix + "VsCulture";
        public static string VisualStudioManifestId { get; private set; } = TelemetryEvents.Prefix + "VsManifestId";
        public static string VisualStudioActiveProjectGuid { get; private set; } = TelemetryEvents.Prefix + "VsActiveProjectGuid";
        public static string SummaryItemEditAction { get; private set; } = TelemetryEvents.Prefix + "ItemEditAction";
        public static string VsProjectCategory { get; private set; } = TelemetryEvents.Prefix + "Category";
        public static string NewItemType { get; private set; } = TelemetryEvents.Prefix + "NewItemType";
        public static string GenSource { get; private set; } = TelemetryEvents.Prefix + "GenSource";
    }
}
