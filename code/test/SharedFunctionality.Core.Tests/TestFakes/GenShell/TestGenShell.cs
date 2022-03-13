// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Core.Gen.Shell;

namespace Microsoft.Templates.Core.Test.TestFakes.GenShell
{
    public class TestGenShell : IGenShell
    {
        public TestGenShell()
        {
            Project = new TestGenShellProject();
            Solution = new TestGenShellSolution();
            Telemetry = new TestGenShellTelemetry();
            UI = new TestGenShellUI();
            VisualStudio = new TestGenShellVisualStudio();
            Certificate = new TestGenShellCertificate();
        }

        public IGenShellProject Project { get; }

        public IGenShellSolution Solution { get; }

        public IGenShellTelemetry Telemetry { get; }

        public IGenShellUI UI { get; }

        public IGenShellVisualStudio VisualStudio { get; }

        public IGenShellCertificate Certificate { get; }
    }
}
