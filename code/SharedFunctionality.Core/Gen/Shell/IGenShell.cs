// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Templates.Core.Gen.Shell
{
    public interface IGenShell
    {
        IGenShellProject Project { get; }

        IGenShellSolution Solution { get; }

        IGenShellTelemetry Telemetry { get; }

        IGenShellUI UI { get; }

        IGenShellVisualStudio VisualStudio { get; }

        IGenShellCertificate Certificate { get; }
    }
}
