// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Templates.Core.Diagnostics
{
    public class TelemetryMetrics
    {
        public static string PagesCount { get; private set; } = TelemetryEvents.Prefix + "PagesCount";

        public static string FeaturesCount { get; private set; } = TelemetryEvents.Prefix + "FeaturesCount";

        public static string ServicesCount { get; private set; } = TelemetryEvents.Prefix + "ServicesCount";

        public static string TestingCount { get; private set; } = TelemetryEvents.Prefix + "TestingCount";

        public static string TimeSpent { get; private set; } = TelemetryEvents.Prefix + "SecTotal";

        public static string ProjectMetricsTimeSpent { get; private set; } = TelemetryEvents.Prefix + "Sec";
    }
}
