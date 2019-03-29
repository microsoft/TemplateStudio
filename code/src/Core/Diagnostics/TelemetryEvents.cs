// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Templates.Core.Diagnostics
{
    public class TelemetryEvents
    {
        public const string Prefix = "Wts";

        public static string ProjectGen { get; private set; } = Prefix + "ProjectGen";

        public static string NewItemGen { get; private set; } = Prefix + "NewItemGen";

        public static string PageGen { get; private set; } = Prefix + "PageGen";

        public static string FeatureGen { get; private set; } = Prefix + "FeatureGen";

        public static string Wizard { get; private set; } = Prefix + "Wizard";

        public static string SessionStart { get; private set; } = Prefix + "SessionStart";

        public static string EditSummaryItem { get; private set; } = Prefix + "EditSummaryItem";
    }
}
