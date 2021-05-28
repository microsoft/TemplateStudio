// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using EnvDTE;
using Microsoft.Templates.Core.Gen.Shell;
using Microsoft.Templates.UI.Threading;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.Templates.UI.VisualStudio.GenShell
{
    public class VsGenShell : IGenShell
    {
        private const string PackagingProjectTypeGuid = "{C7167F0D-BC9F-4E6E-AFE1-012C56B48DB5}";

        private readonly AsyncLazy<DTE> _dte = new AsyncLazy<DTE>(
         async () =>
         {
             await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
             return ServiceProvider.GlobalProvider.GetService(typeof(DTE)) as DTE;
         },
         SafeThreading.JoinableTaskFactory);

        private readonly AsyncLazy<IVsSolution> _vssolution = new AsyncLazy<IVsSolution>(
             async () =>
             {
                 await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                 return ServiceProvider.GlobalProvider.GetService(typeof(SVsSolution)) as IVsSolution;
             },
             SafeThreading.JoinableTaskFactory);

        public VsGenShell()
        {
            Project = new VsGenShellProject(_vssolution, _dte, PackagingProjectTypeGuid);
            Solution = new VsGenShellSolution(_vssolution, _dte, PackagingProjectTypeGuid);
            Telemetry = new VsGenShellTelemetry();
            UI = new VsGenShellUI(_dte);
            VisualStudio = new VsGenShellVisualStudio(_dte);
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
