// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen.Shell;

namespace Microsoft.Templates.UI.VisualStudio.GenShell
{
    public class VsGenShellTelemetry : IGenShellTelemetry
    {
        private readonly Lazy<VSTelemetryService> telemetryService = new Lazy<VSTelemetryService>(() => new VSTelemetryService());

        private VSTelemetryService VSTelemetryService => telemetryService.Value;

        public VSTelemetryInfo GetVSTelemetryInfo()
        {
            return VSTelemetryService.VsTelemetryIsOptedIn();
        }

        public void SafeTrackNewItemVsTelemetry(Dictionary<string, string> properties, string pages, string features, string services, string testing, Dictionary<string, double> metrics, bool success = true)
        {
            VSTelemetryService.SafeTrackNewItemVsTelemetry(properties, pages, features, metrics, success);
        }

        public void SafeTrackProjectVsTelemetry(Dictionary<string, string> properties, string pages, string features, string services, string testing, Dictionary<string, double> metrics, bool success = true)
        {
            VSTelemetryService.SafeTrackProjectVsTelemetry(properties, pages, features, metrics, success);
        }

        public void SafeTrackWizardCancelledVsTelemetry(Dictionary<string, string> properties, bool success = true)
        {
            VSTelemetryService.SafeTrackWizardCancelledVsTelemetry(properties, success);
        }
    }
}
