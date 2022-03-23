// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen.Shell;

namespace Microsoft.Templates.Fakes.GenShell
{
    public class FakeGenShellTelemetry : IGenShellTelemetry
    {
        public VSTelemetryInfo GetVSTelemetryInfo()
        {
            return new VSTelemetryInfo()
            {
                OptedIn = true,
                VisualStudioCulture = string.Empty,
                VisualStudioEdition = string.Empty,
                VisualStudioExeVersion = string.Empty,
                VisualStudioManifestId = string.Empty,
            };
        }

        public void SafeTrackNewItemVsTelemetry(Dictionary<string, string> properties, string pages, string features, string services, string testing, Dictionary<string, double> metrics, bool success = true)
        {
        }

        public void SafeTrackProjectVsTelemetry(Dictionary<string, string> properties, string pages, string features, string services, string testing, Dictionary<string, double> metrics, bool success = true)
        {
        }

        public void SafeTrackWizardCancelledVsTelemetry(Dictionary<string, string> properties, bool success = true)
        {
        }
    }
}
