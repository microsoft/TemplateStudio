// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using EnvDTE;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Gen.Shell;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.Threading;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TemplateWizard;
using Microsoft.VisualStudio.Threading;
using Task = System.Threading.Tasks.Task;

namespace Microsoft.Templates.UI.VisualStudio.GenShell
{
    public class VsGenShellUI : IGenShellUI
    {
        private readonly AsyncLazy<DTE> _dte;
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

        public VsGenShellUI(AsyncLazy<DTE> dte)
        {
            _dte = dte;
        }

        public void CancelWizard(bool back = true)
        {
            if (back)
            {
                throw new WizardBackoutException();
            }
            else
            {
                throw new WizardCancelledException();
            }
        }

        public void OpenItems(params string[] itemsFullPath)
        {
            SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                await OpenItemsAsync(itemsFullPath);
            });
        }

        private async Task OpenItemsAsync(params string[] itemsFullPath)
        {
            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
            var dte = await _dte.GetValueAsync();
            if (itemsFullPath == null || itemsFullPath.Length == 0)
            {
                return;
            }

            foreach (var item in itemsFullPath)
            {
                switch (Path.GetExtension(item).ToUpperInvariant())
                {
                    case ".XAML":
                        dte.ItemOperations.OpenFile(item, EnvDTE.Constants.vsViewKindDesigner);
                        break;

                    default:
                        if (!item.EndsWith(".xaml.cs", StringComparison.OrdinalIgnoreCase))
                        {
                            dte.ItemOperations.OpenFile(item, EnvDTE.Constants.vsViewKindPrimary);
                        }

                        break;
                }
            }
        }

        public void OpenProjectOverview()
        {
            SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                await OpenProjectOverviewAsync();
            });
        }

        private async Task OpenProjectOverviewAsync()
        {
            if (GenContext.CurrentPlatform == Platforms.Uwp)
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                var dte = await _dte.GetValueAsync();
                dte.Events.SolutionEvents.Opened += SolutionEvents_Opened;
            }
        }

        public void ShowModal(IWindow shell)
        {
            if (shell is System.Windows.Window dialog)
            {
                SafeThreading.JoinableTaskFactory.Run(async () =>
                {
                    await ShowModalAsync(shell);
                });
            }
        }

        private async Task ShowModalAsync(IWindow shell)
        {
            if (shell is System.Windows.Window dialog)
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();

                // get the owner of this dialog
                var uiShell = await _uiShell.GetValueAsync();
                uiShell.GetDialogOwnerHwnd(out IntPtr hwnd);

                dialog.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

                uiShell.EnableModeless(0);

                try
                {
                    WindowHelper.ShowModal(dialog, hwnd);
                }
                finally
                {
                    // This will take place after the window is closed.
                    uiShell.EnableModeless(1);
                }
            }
        }

        public void ShowStatusBarMessage(string message)
        {
            try
            {
                SafeThreading.JoinableTaskFactory.Run(async () =>
                {
                    await ShowStatusBarMessageAsync(message);
                });
            }
            catch (Exception ex)
            {
                AppHealth.Current.Error.TrackAsync($"{StringRes.ErrorVsGenShellShowStatusBarMessageMessage} {ex}").FireAndForget();
            }
        }

        public async Task ShowStatusBarMessageAsync(string message)
        {
            try
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                var dte = await _dte.GetValueAsync();
                dte.StatusBar.Text = message;
            }
            catch (Exception ex)
            {
                AppHealth.Current.Error.TrackAsync($"{StringRes.ErrorVsGenShellShowStatusBarMessageMessage} {ex}").FireAndForget();
            }
        }

        public void ShowTaskList()
        {
            ShowTaskListAsync().FireAndForget();
        }

        private async Task ShowTaskListAsync()
        {
            // JAVIERS: DELAY THIS EXECUTION TO OPEN THE WINDOW AFTER EVERYTHING IS LOADED
            await System.Threading.Tasks.Task.Delay(1000);

            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
            var dte = await _dte.GetValueAsync();

            var window = dte.Windows.Item(EnvDTE.Constants.vsWindowKindTaskList);

            window.Activate();
        }

        public void WriteOutput(string data)
        {
            SafeThreading.JoinableTaskFactory.RunAsync(async () =>
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                var output = await _outputPane.GetValueAsync();
                output.Write(data);
            });
        }

        private async void SolutionEvents_Opened()
        {
            try
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();

                var dte = await _dte.GetValueAsync();
                var solutionExplorer = dte.Windows.Item(EnvDTE.Constants.vsext_wk_SProjectWindow).Object as UIHierarchy;
                var projectNode = solutionExplorer.UIHierarchyItems.Item(1)?.UIHierarchyItems.Item(1);
                projectNode.Select(vsUISelectionType.vsUISelectionTypeSelect);

                dte.ExecuteCommand("Project.Overview");
                dte.Events.SolutionEvents.Opened -= SolutionEvents_Opened;
            }
            catch (Exception)
            {
                AppHealth.Current.Error.TrackAsync(StringRes.ErrorUnableToOpenProjectOverview).FireAndForget();
            }
        }
    }
}
