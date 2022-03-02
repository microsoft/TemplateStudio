// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.Templates.Core.Diagnostics;

namespace Microsoft.Templates.Core.Gen.Shell
{
    public interface IGenShellTelemetry
    {
        VSTelemetryInfo GetVSTelemetryInfo();

        void SafeTrackProjectVsTelemetry(Dictionary<string, string> properties, string pages, string features, string services, string testing, Dictionary<string, double> metrics, bool success = true);

        void SafeTrackNewItemVsTelemetry(Dictionary<string, string> properties, string pages, string features, string services, string testing, Dictionary<string, double> metrics, bool success = true);

        void SafeTrackWizardCancelledVsTelemetry(Dictionary<string, string> properties, bool success = true);
    }
}
