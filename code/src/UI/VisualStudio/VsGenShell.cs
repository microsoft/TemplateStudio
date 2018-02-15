// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using EnvDTE;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.Threading;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Setup.Configuration;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TemplateWizard;
using NuGet.VisualStudio;

namespace Microsoft.Templates.UI.VisualStudio
{
    public class VsGenShell : GenShell
    {
        private Lazy<DTE> _dte = new Lazy<DTE>(() => ServiceProvider.GlobalProvider.GetService(typeof(DTE)) as DTE, true);

        private DTE Dte => _dte.Value;

        private string _vsVersionInstance = string.Empty;

        private string _vsProductVersion = string.Empty;

        private Lazy<IVsUIShell> _uiShell = new Lazy<IVsUIShell>(
            () =>
            {
                SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                return ServiceProvider.GlobalProvider.GetService(typeof(SVsUIShell)) as IVsUIShell;
            },
            true);

        private IVsUIShell UIShell => _uiShell.Value;

        private Lazy<IVsSolution> _vssolution = new Lazy<IVsSolution>(
            () =>
            {
                SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                return ServiceProvider.GlobalProvider.GetService(typeof(SVsSolution)) as IVsSolution;
            },
            true);

        private IVsSolution VSSolution => _vssolution.Value;

        private Lazy<VsOutputPane> _outputPane = new Lazy<VsOutputPane>(() => new VsOutputPane());

        private VsOutputPane OutputPane => _outputPane.Value;

        public override void AddItems(params string[] itemsFullPath)
        {
            if (itemsFullPath == null || itemsFullPath.Length == 0)
            {
                return;
            }

            var proj = GetActiveProject();
            if (proj != null && proj.ProjectItems != null)
            {
                foreach (var item in itemsFullPath)
                {
                    proj.ProjectItems.AddFromFile(item);
                }

                proj.Save();
            }
            else
            {
                AppHealth.Current.Error.TrackAsync(StringRes.ErrorUnableAddItemsToProject).FireAndForget();
            }
        }

        public override void RefreshProject()
        {
            try
            {
                var proj = GetActiveProject();

                if (proj != null)
                {
                    var path = proj.FullName;

                    Dte.Solution.Remove(proj);
                    Dte.Solution.AddFromFile(path);
                }
            }
            catch (Exception)
            {
                // WE GET AN EXCEPTION WHEN THERE ISN'T A PROJECT LOADED
                AppHealth.Current.Info.TrackAsync(StringRes.ErrorUnableToRefreshProject).FireAndForget();
            }
        }

        public override bool SetActiveConfigurationAndPlatform(string configurationName, string platformName)
        {
            foreach (SolutionConfiguration solConfiguration in Dte.Solution.SolutionBuild.SolutionConfigurations)
            {
                if (solConfiguration.Name == configurationName)
                {
                    foreach (SolutionContext context in solConfiguration.SolutionContexts)
                    {
                        if (context.PlatformName == platformName)
                        {
                            solConfiguration.Activate();

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public override void AddProjectToSolution(string projectFullPath)
        {
            try
            {
                Dte.Solution.AddFromFile(projectFullPath);
            }
            catch (Exception)
            {
                // WE GET AN EXCEPTION WHEN THERE ISN'T A SOLUTION LOADED
                AppHealth.Current.Info.TrackAsync(StringRes.ErrorUnableAddProjectToSolution).FireAndForget();
            }
        }

        public override string GetActiveProjectNamespace()
        {
            var p = GetActiveProject();

            if (p != null)
            {
                return p.Properties.GetSafeValue("DefaultNamespace");
            }

            return null;
        }

        public override void CleanSolution()
        {
            Dte.Solution.SolutionBuild.Clean();
        }

        public override void SaveSolution()
        {
            Dte.Solution.SaveAs(Dte.Solution.FullName);
        }

        public override void ShowStatusBarMessage(string message)
        {
            try
            {
                Dte.StatusBar.Text = message;
            }
            catch (Exception ex)
            {
                AppHealth.Current.Error.TrackAsync($"{StringRes.ErrorVsGenShellShowStatusBarMessageMessage} {ex.ToString()}").FireAndForget();
            }
        }

        public override void ShowTaskList()
        {
            ShowTaskListAsync().FireAndForget();
        }

        public override void ShowModal(System.Windows.Window dialog)
        {
            SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();

            // get the owner of this dialog
            UIShell.GetDialogOwnerHwnd(out IntPtr hwnd);

            dialog.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

            UIShell.EnableModeless(0);

            try
            {
                WindowHelper.ShowModal(dialog, hwnd);
            }
            finally
            {
                // This will take place after the window is closed.
                UIShell.EnableModeless(1);
            }
        }

        public override void CancelWizard(bool back = true)
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

        public override string GetActiveProjectGuid()
        {
            SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();

            var p = GetActiveProject();

            if (p != null)
            {
                VSSolution.GetProjectOfUniqueName(p.FullName, out IVsHierarchy hierarchy);
                if (hierarchy != null)
                {
                    hierarchy.GetGuidProperty(VSConstants.VSITEMID_ROOT, (int)__VSHPROPID.VSHPROPID_ProjectIDGuid, out Guid projectGuid);

                    return projectGuid.ToString();
                }
            }

            return string.Empty;
        }

        protected override string GetSelectedItemPath()
        {
            if (Dte.SelectedItems.Count > 0)
            {
                var item = Dte.SelectedItems.Item(1);

                if (item.Project != null)
                {
                    return Path.GetDirectoryName(item.Project.FullName);
                }

                if (item.ProjectItem != null)
                {
                    string fullPath = $"{item.ProjectItem.Properties.GetSafeValue("FullPath")}";

                    return Path.GetDirectoryName(fullPath);
                }
            }

            return GetActiveProjectPath();
        }

        public override string GetActiveProjectName()
        {
            var p = GetActiveProject();

            return p?.Name;
        }

        public override string GetActiveProjectPath()
        {
            var p = GetActiveProject();

            if (p != null)
            {
                return Path.GetDirectoryName(p.FileName);
            }
            else
            {
                return null;
            }
        }

        public override string GetActiveProjectLanguage()
        {
            var p = GetActiveProject();

            if (p != null)
            {
                switch (Path.GetExtension(p.SafeGetFileName()))
                {
                    case ".csproj":
                        return ProgrammingLanguages.CSharp;

                    case ".vbproj":
                        return ProgrammingLanguages.VisualBasic;

                    default:
                        return string.Empty;
                }
            }
            else
            {
                return null;
            }
        }

        private Project GetActiveProject()
        {
            Project p = null;

            try
            {
                if (_dte != null)
                {
                    Array projects = (Array)Dte.ActiveSolutionProjects;

                    if (projects?.Length >= 1)
                    {
                        p = (Project)projects.GetValue(0);
                    }
                }
            }
            catch (Exception)
            {
                // WE GET AN EXCEPTION WHEN THERE ISN'T A PROJECT LOADED
                p = null;
            }

            return p;
        }

        private async System.Threading.Tasks.Task ShowTaskListAsync()
        {
            // JAVIERS: DELAY THIS EXECUTION TO OPEN THE WINDOW AFTER EVERYTHING IS LOADED
            await System.Threading.Tasks.Task.Delay(1000);

            var window = Dte.Windows.Item(EnvDTE.Constants.vsWindowKindTaskList);

            window.Activate();
        }

        public override void WriteOutput(string data)
        {
            OutputPane.Write(data);
        }

        public override void CloseSolution()
        {
            Dte.Solution.Close();
        }

        public override void RestorePackages()
        {
            try
            {
                if (IsInternetAvailable())
                {
                    var componentModel = (IComponentModel)Package.GetGlobalService(typeof(SComponentModel));

                    var installer = componentModel.GetService<IVsPackageInstaller>();
                    var uninstaller = componentModel.GetService<IVsPackageUninstaller>();
                    var installerServices = componentModel.GetService<IVsPackageInstallerServices>();

                    var installedPackages = installerServices.GetInstalledPackages().ToList();
                    var activeProject = GetActiveProject();

                    var p = installedPackages.FirstOrDefault();

                    if (p != null)
                    {
                        uninstaller.UninstallPackage(activeProject, p.Id, false);
                        installer.InstallPackage("All", activeProject, p.Id, p.VersionString, true);
                    }
                }
                else
                {
                    AppHealth.Current.Warning.TrackAsync(StringRes.ErrorVsGenShellRestorePackagesWarningMessage).FireAndForget();
                }
            }
            catch (Exception ex)
            {
                AppHealth.Current.Error.TrackAsync($"{StringRes.ErrorVsGenShellRestorePackagesErrorMessage} {ex.ToString()}").FireAndForget();
            }
        }

        public override void CollapseSolutionItems()
        {
            try
            {
                var solutionExplorer = Dte.Windows.Item(EnvDTE.Constants.vsext_wk_SProjectWindow).Object as UIHierarchy;
                var projectNode = solutionExplorer.UIHierarchyItems.Item(1)?.UIHierarchyItems.Item(1);

                foreach (UIHierarchyItem item in projectNode.UIHierarchyItems)
                {
                    Collapse(item);
                }
            }
            catch (Exception ex)
            {
                AppHealth.Current.Error.TrackAsync($"{StringRes.ErrorVsGenShellCollapseSolutionItemsMessage} {ex.ToString()}").FireAndForget();
            }
        }

        public override Guid GetVsProjectId()
        {
            SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();

            var project = GetActiveProject();
            Guid projectGuid = Guid.Empty;
            try
            {
                if (project != null)
                {
                    var solution = ServiceProvider.GlobalProvider.GetService(typeof(SVsSolution)) as IVsSolution;

                    solution.GetProjectOfUniqueName(project.FullName, out IVsHierarchy hierarchy);

                    if (hierarchy != null)
                    {
                        hierarchy.GetGuidProperty(
                                    VSConstants.VSITEMID_ROOT,
                                    (int)__VSHPROPID.VSHPROPID_ProjectIDGuid,
                                    out projectGuid);
                    }
                }
            }
            catch
            {
                projectGuid = Guid.Empty;
            }

            return projectGuid;
        }

        private void Collapse(UIHierarchyItem item)
        {
            foreach (UIHierarchyItem subitem in item.UIHierarchyItems)
            {
                Collapse(subitem);
            }

            item.UIHierarchyItems.Expanded = false;
        }

        private static bool IsInternetAvailable()
        {
            bool internet = NetworkInterface.GetIsNetworkAvailable();
            if (internet)
            {
                try
                {
                    // Based on https://technet.microsoft.com/en-us/library/cc766017(v=ws.10).aspx
                    using (var client = new System.Net.WebClient())
                    {
                        var ncsi = client.DownloadString("http://www.msftncsi.com/ncsi.txt");
                        internet = ncsi == "Microsoft NCSI";
                    }
                }
                catch
                {
                    internet = false;
                }
            }

            return internet;
        }

        public override void OpenItems(params string[] itemsFullPath)
        {
            if (itemsFullPath == null || itemsFullPath.Length == 0)
            {
                return;
            }

            foreach (var item in itemsFullPath)
            {
                switch (Path.GetExtension(item).ToUpperInvariant())
                {
                    case ".XAML":
                        Dte.ItemOperations.OpenFile(item, EnvDTE.Constants.vsViewKindDesigner);
                        break;

                    default:
                        if (!item.EndsWith(".xaml.cs", StringComparison.OrdinalIgnoreCase))
                        {
                            Dte.ItemOperations.OpenFile(item, EnvDTE.Constants.vsViewKindPrimary);
                        }

                        break;
                }
            }
        }

        public override bool IsDebuggerEnabled()
        {
            return Dte.Debugger.CurrentMode != dbgDebugMode.dbgDesignMode;
        }

        public override string GetVsVersionAndInstance()
        {
            if (string.IsNullOrEmpty(_vsVersionInstance))
            {
                ISetupConfiguration configuration = new SetupConfiguration() as ISetupConfiguration;
                ISetupInstance instance = configuration.GetInstanceForCurrentProcess();
                string version = instance.GetInstallationVersion();
                string instanceId = instance.GetInstanceId();
                _vsVersionInstance = $"{version}-{instanceId}";
            }

            return _vsVersionInstance;
        }

        public override string GetVsVersion()
        {
            if (string.IsNullOrEmpty(_vsProductVersion))
            {
                ISetupConfiguration configuration = new SetupConfiguration() as ISetupConfiguration;
                ISetupInstance instance = configuration.GetInstanceForCurrentProcess();
                _vsProductVersion = instance.GetInstallationVersion();
            }

            return _vsProductVersion;
        }
    }
}
