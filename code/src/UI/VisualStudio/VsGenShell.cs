// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
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
using Microsoft.VisualStudio.Shell.Flavor;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TemplateWizard;
using NuGet.VisualStudio;
using VSLangProj;

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

            var filesByProject = ResolveProjectFiles(itemsFullPath);

            foreach (var projectFile in filesByProject)
            {
                var proj = GetProjectByPath(projectFile.Key);
                if (proj != null && proj.ProjectItems != null)
                {
                    var isSharedProject = Path.GetExtension(proj.FileName).Equals(".shproj", StringComparison.OrdinalIgnoreCase);
                    foreach (var file in projectFile.Value)
                    {
                        var newItem = proj.ProjectItems.AddFromFile(file);

                        if (isSharedProject && Path.GetExtension(file).Equals(".xaml", StringComparison.OrdinalIgnoreCase))
                        {
                            newItem.Properties.Item("ItemType").Value = "EmbeddedResource";
                            newItem.Properties.Item("Generator").Value = "MSBuild:UpdateDesignTimeXaml";
                        }
                    }

                    proj.Save();
                }
            }
        }

        public override void RefreshProject(string projectPath)
        {
            try
            {
                var proj = GetProjectByPath(projectPath);

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

        public override void SetDefaultSolutionConfiguration(string configurationName, string platformName, string projectGuid)
        {
            try
            {
                var defaultProject = GetProjectByGuid(projectGuid);

                SetActiveConfigurationAndPlatform(configurationName, platformName, defaultProject);
                SetStartupProject(defaultProject);
            }
            catch (Exception ex)
            {
                AppHealth.Current.Error.TrackAsync($"{StringRes.ErrorUnableToSetDefaultConfiguration} {ex.ToString()}").FireAndForget();
            }
        }

        private bool SetActiveConfigurationAndPlatform(string configurationName, string platformName, Project project)
        {
            foreach (SolutionConfiguration solConfiguration in Dte.Solution?.SolutionBuild?.SolutionConfigurations)
            {
                if (solConfiguration.Name == configurationName)
                {
                    foreach (SolutionContext context in solConfiguration.SolutionContexts)
                    {
                        if (context.PlatformName == platformName && context.ProjectName == project?.UniqueName)
                        {
                            solConfiguration.Activate();
                        }
                    }
                }
            }

            return false;
        }

        private void SetStartupProject(Project project)
        {
            if (project != null)
            {
                var solution = GetSolution();
                solution.Properties.Item("StartupProject").Value = project.Name;
            }
        }

        public override void AddProjectToSolution(string projectPath, bool usesAnyCpu)
        {
            try
            {
                Dte.Solution.AddFromFile(projectPath);
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

        public override void OpenProjectOverview()
        {
            Dte.Events.SolutionEvents.Opened += SolutionEvents_Opened;
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

        public override string GetActiveProjectTypeGuids()
        {
            var project = GetActiveProject();
            return GetProjectTypeGuid(project);
        }

        private string GetProjectTypeGuid(Project project)
        {
            SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();

            if (project != null)
            {
                VSSolution.GetProjectOfUniqueName(project.FullName, out IVsHierarchy hierarchy);

                if (hierarchy is IVsAggregatableProjectCorrected aggregatableProject)
                {
                    aggregatableProject.GetAggregateProjectTypeGuids(out string projTypeGuids);

                    return projTypeGuids;
                }
            }

            return string.Empty;
        }

        private Project GetProjectByGuid(string projectTypeGuid)
        {
            foreach (var p in Dte?.Solution?.Projects?.Cast<Project>())
            {
                var projectGuid = GetProjectTypeGuid(p);

                if (projectGuid.ToUpperInvariant().Split(';').Contains($"{{{projectTypeGuid}}}"))
                {
                    return p;
                }
            }

            return null;
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

        public override string GetSolutionPath()
        {
            var s = GetSolution();
            if (s != null)
            {
                return Path.GetDirectoryName(s.FileName);
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

        private Project GetProjectByPath(string projFile)
        {
            Project p = null;
            try
            {
                if (_dte != null)
                {
                    SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();

                    VSSolution.GetProjectOfUniqueName(projFile, out IVsHierarchy hierarchy);
                    ErrorHandler.ThrowOnFailure(hierarchy.GetProperty(VSConstants.VSITEMID_ROOT, (int)__VSHPROPID.VSHPROPID_ExtObject, out object obj));
                    p = (Project)obj;
                }
            }
            catch (Exception ex)
            {
                AppHealth.Current.Error.TrackAsync(StringRes.ErrorUnableAddItemsToProject, ex).FireAndForget();
            }

            return p;
        }

        private Solution GetSolution()
        {
            Solution s = null;

            try
            {
                if (_dte != null)
                {
                    s = Dte.Solution;
                }
            }
            catch (Exception)
            {
                // WE GET AN EXCEPTION WHEN THERE ISN'T A PROJECT LOADED
                s = null;
            }

            return s;
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

        private void SolutionEvents_Opened()
        {
            Dte.ExecuteCommand("Project.Overview");
            Dte.Events.SolutionEvents.Opened -= SolutionEvents_Opened;
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

        public override bool IsBuildInProgress()
        {
            return Dte.Solution.SolutionBuild.BuildState == vsBuildState.vsBuildStateInProgress;
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

        public override void AddReferenceToProject(string projectPath, string referenceToAdd)
        {
            var referenceProject = GetProjectByPath(referenceToAdd);

            var project = GetProjectByPath(projectPath);

            var proj = (VSProject)project.Object;
            proj.References.AddProject(referenceProject);
            project.Save();
        }
    }
}
