// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.Templates.UI.Threading;
using Microsoft.VisualStudio.Setup.Configuration;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.Templates.UI.VisualStudio.GenShell
{
    public class VsShellService
    {
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

        private readonly AsyncLazy<IVsUIShell> _uiShell = new AsyncLazy<IVsUIShell>(
          async () =>
          {
              await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
              return ServiceProvider.GlobalProvider.GetService(typeof(SVsUIShell)) as IVsUIShell;
          },
          SafeThreading.JoinableTaskFactory);

        private readonly AsyncLazy<VsOutputPane> _outputPane = new AsyncLazy<VsOutputPane>(
            async () =>
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                var pane = new VsOutputPane();
                await pane.InitializeAsync();
                return pane;
            },
            SafeThreading.JoinableTaskFactory);

        private readonly Lazy<ISetupInstance2> _vsInstance = new Lazy<ISetupInstance2>(() =>
        {
            var setupConfiguration = new SetupConfiguration();
            return setupConfiguration.GetInstanceForCurrentProcess() as ISetupInstance2;
        });

        internal async Task<DTE> GetDteAsync() => await _dte.GetValueAsync();

        internal async Task<IVsSolution> GetVsSolutionAsync() => await _vssolution.GetValueAsync();

        internal async Task<IVsUIShell> GetUIShellAsync() => await _uiShell.GetValueAsync();

        internal async Task<VsOutputPane> GetVsOutputPaneAsync() => await _outputPane.GetValueAsync();

        internal ISetupInstance2 GetSetupInstance() => _vsInstance.Value;
    }
}
