// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Templates.UI.VisualStudio
{
    public class VsTelemetryEvents
    {
        public const string Prefix = "TS.";

        public static string TSGen { get; private set; } = "vs/templatestudio/ts-generated";

        public static string TSWizard { get; private set; } = "vs/templatestudio/ts-wizard";
    }
}
