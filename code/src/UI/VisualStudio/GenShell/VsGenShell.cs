// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Core.Gen.Shell;

namespace Microsoft.Templates.UI.VisualStudio.GenShell
{
    public class VsGenShell : IGenShell
    {
        public VsGenShell()
        {
            var vsShellService = new VsShellService();
            Project = new VsGenShellProject(vsShellService);
            Solution = new VsGenShellSolution(vsShellService);
            Telemetry = new VsGenShellTelemetry();
            UI = new VsGenShellUI(vsShellService);
            VisualStudio = new VsGenShellVisualStudio(vsShellService);
            Certificate = new VsGenShellCertificate();
        }

        public IGenShellProject Project { get; }

        public IGenShellSolution Solution { get; }

        public IGenShellTelemetry Telemetry { get; }

        public IGenShellUI UI { get; }

        public IGenShellVisualStudio VisualStudio { get; }

        public IGenShellCertificate Certificate { get; }
    }
}
