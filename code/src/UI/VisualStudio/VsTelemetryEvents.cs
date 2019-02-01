// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Templates.UI.VisualStudio
{
    public class VsTelemetryEvents
    {
        public const string Prefix = "Wts.";

        public static string WtsGen { get; private set; } = "vs/windowstemplatestudio/wts-generated";

        public static string WtsWizard { get; private set; } = "vs/windowstemplatestudio/wts-wizard";
    }
}
