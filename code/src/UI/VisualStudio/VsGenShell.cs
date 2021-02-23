// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.Build.Utilities;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Extensions;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.Threading;
using Microsoft.Templates.Utilities.Services;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.ProjectSystem.Properties;
using Microsoft.VisualStudio.Setup.Configuration;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Flavor;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TemplateWizard;
using Microsoft.VisualStudio.Threading;
using NuGet.VisualStudio;
using VSLangProj;
using Task = System.Threading.Tasks.Task;

namespace Microsoft.Templates.UI.VisualStudio
{
    public class VsGenShell : GenShell
    {
        private const string PackagingProjectTypeGuid = "{C7167F0D-BC9F-4E6E-AFE1-012C56B48DB5}";

        private readonly AsyncLazy<DTE> _dte = new AsyncLazy<DTE>(
             async () =>
             {
                 await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                 return ServiceProvider.GlobalProvider.GetService(typeof(DTE)) as DTE;
             },
             SafeThreading.JoinableTaskFactory);

        private readonly AsyncLazy<IVsUIShell> _uiShell = new AsyncLazy<IVsUIShell>(
           async () =>
           {
               await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
               return ServiceProvider.GlobalProvider.GetService(typeof(SVsUIShell)) as IVsUIShell;
           },
           SafeThreading.JoinableTaskFactory);

        private readonly AsyncLazy<IVsSolution> _vssolution = new AsyncLazy<IVsSolution>(
            async () =>
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                return ServiceProvider.GlobalProvider.GetService(typeof(SVsSolution)) as IVsSolution;
            },
            SafeThreading.JoinableTaskFactory);

        private readonly Lazy<ISetupInstance2> vsInstance = new Lazy<ISetupInstance2>(() =>
        {
            var setupConfiguration = new SetupConfiguration();
            return setupConfiguration.GetInstanceForCurrentProcess() as ISetupInstance2;
        });

        private readonly List<string> installedPackageIds = new List<string>();

        private readonly AsyncLazy<VsOutputPane> _outputPane = new AsyncLazy<VsOutputPane>(
            async () =>
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                var pane = new VsOutputPane();
                await pane.InitializeAsync();
                return pane;
            },
            SafeThreading.JoinableTaskFactory);

        private readonly Lazy<VSTelemetryService> telemetryService = new Lazy<VSTelemetryService>(() => new VSTelemetryService());

        private string _vsVersionInstance = string.Empty;

        private string _vsProductVersion = string.Empty;

        private VSTelemetryService VSTelemetryService => telemetryService.Value;

        private async Task AddProjectAsync(string project)
        {
            try
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                GenContext.ToolBox.Shell.ShowStatusBarMessage(string.Format(StringRes.StatusAddingProject, Path.GetFileName(project)));

                var dte = await _dte.GetValueAsync();
                dte.Solution.AddFromFile(project);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(StringRes.ErrorAddingProject, project), ex);
            }
        }

        private async Task AddItemsAsync(string projPath, IEnumerable<string> projFiles)
        {
            try
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                var proj = await GetProjectByPathAsync(projPath);
                if (proj != null && proj.ProjectItems != null)
                {
                    foreach (var file in projFiles)
                    {
                        GenContext.ToolBox.Shell.ShowStatusBarMessage(string.Format(StringRes.StatusAddingItem, Path.GetFileName(file)));

                        var newItem = proj.ProjectItems.AddFromFile(file);

                        if (GenContext.CurrentLanguage == ProgrammingLanguages.Cpp && Path.GetExtension(file) == ".xaml" && File.Exists(file.Replace("xaml", "idl")))
                        {
                            newItem.ProjectItems.AddFromFile(file.Replace("xaml", "idl"));
                        }
                    }

                    proj.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(StringRes.ErrorAddingItemsToProject, projPath), ex);
            }
        }

        internal List<string> GetInstalledPackageIds()
        {
            if (!installedPackageIds.Any())
            {
                foreach (var package in this.vsInstance.Value.GetPackages())
                {
                    installedPackageIds.Add(package.GetId());
                }
            }

            return installedPackageIds;
        }

        public override bool IsSdkInstalled(string version)
        {
            var sdks = ToolLocationHelper.GetTargetPlatformSdks();

            foreach (var sdk in sdks)
            {
                var versions = ToolLocationHelper.GetPlatformsForSDK(sdk.TargetPlatformIdentifier, sdk.TargetPlatformVersion);
                if (versions.Any(v => v.Contains(version)))
                {
                    return true;
                }
            }

            return false;
        }

        public override void SetDefaultSolutionConfiguration(string configurationName, string platformName, string projectName)
        {
            SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                await SetDefaultSolutionConfigurationAsync(configurationName, platformName, projectName);
            });
        }

        public async Task SetDefaultSolutionConfigurationAsync(string configurationName, string platformName, string projectName)
        {
            try
            {
                var startupProject = await GetProjectByProjectTypeGuidAsync(PackagingProjectTypeGuid);
                if (startupProject == null)
                {
                    startupProject = await GetProjectByNameAsync(projectName);
                }

                await SetActiveConfigurationAndPlatformAsync(configurationName, platformName, startupProject);
                await SetStartupProjectAsync(startupProject);
            }
            catch (Exception ex)
            {
                AppHealth.Current.Error.TrackAsync($"{StringRes.ErrorUnableToSetDefaultConfiguration} {ex}").FireAndForget();
            }
        }

        public override string GetActiveProjectNamespace()
        {
            return SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                return await GetActiveProjectNamespaceAsync();
            });
        }

        public async Task<string> GetActiveProjectNamespaceAsync()
        {
            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
            var p = await GetActiveProjectAsync();

            if (p != null && p.Properties != null)
            {
                return p.Properties.Item("RootNamespace")?.Value?.ToString();
            }

            return null;
        }

        public override void ShowStatusBarMessage(string message)
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

        public override void ShowTaskList()
        {
            ShowTaskListAsync().FireAndForget();
        }

        public override void OpenProjectOverview()
        {
            SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                await OpenProjectOverviewAsync();
            });
        }

        public async Task OpenProjectOverviewAsync()
        {
            if (GenContext.CurrentPlatform == Platforms.Uwp)
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                var dte = await _dte.GetValueAsync();
                dte.Events.SolutionEvents.Opened += SolutionEvents_Opened;
            }
        }

        public override void ShowModal(IWindow shell)
        {
            if (shell is System.Windows.Window dialog)
            {
                SafeThreading.JoinableTaskFactory.Run(async () =>
                {
                    await ShowModalAsync(shell);
                });
            }
        }

        public async Task ShowModalAsync(IWindow shell)
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

        public override string GetActiveProjectTypeGuids()
        {
            return SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                return await GetActiveProjectTypeGuidsAsync();
            });
        }

        public async Task<string> GetActiveProjectTypeGuidsAsync()
        {
            var project = await GetActiveProjectAsync();
            return await GetProjectTypeGuidAsync(project);
        }

        public override string GetActiveProjectName()
        {
            return SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                return await GetActiveProjectNamespaceAsync();
            });
        }

        public async Task<string> GetActiveProjectNameAsync()
        {
            var p = await GetActiveProjectAsync();

            return p?.Name;
        }

        public override string GetActiveProjectPath()
        {
            return SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                return await GetActiveProjectPathAsync();
            });
        }

        public async Task<string> GetActiveProjectPathAsync()
        {
            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
            var p = await GetActiveProjectAsync();

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
            return SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                return await GetActiveProjectLanguageAsync();
            });
        }

        public async Task<string> GetActiveProjectLanguageAsync()
        {
            var p = await GetActiveProjectAsync();

            if (p != null)
            {
                switch (Path.GetExtension(p.SafeGetFileName()))
                {
                    case ".csproj":
                        return ProgrammingLanguages.CSharp;
                    case ".vbproj":
                        return ProgrammingLanguages.VisualBasic;
                    case ".vcxproj":
                        return ProgrammingLanguages.Cpp;
                    default:
                        return string.Empty;
                }
            }
            else
            {
                return null;
            }
        }

        public override void WriteOutput(string data)
        {
            SafeThreading.JoinableTaskFactory.RunAsync(async () =>
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                var output = await _outputPane.GetValueAsync();
                output.Write(data);
            });
        }

        public override void CloseSolution()
        {
            SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                await CloseSolutionAsync();
            });
        }

        public async Task CloseSolutionAsync()
        {
            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
            var dte = await _dte.GetValueAsync();
            dte.Solution.Close();
        }

        public override void CollapseSolutionItems()
        {
            try
            {
                SafeThreading.JoinableTaskFactory.Run(async () =>
                {
                    await CollapseSolutionItemsAsync();
                });
            }
            catch (Exception ex)
            {
                AppHealth.Current.Error.TrackAsync($"{StringRes.ErrorVsGenShellCollapseSolutionItemsMessage} {ex}").FireAndForget();
            }
        }

        public async Task CollapseSolutionItemsAsync()
        {
            try
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                var dte = await _dte.GetValueAsync();
                var solutionExplorer = dte.Windows.Item(EnvDTE.Constants.vsext_wk_SProjectWindow).Object as UIHierarchy;
                var projectNode = solutionExplorer.UIHierarchyItems.Item(1)?.UIHierarchyItems.Item(1);

                foreach (UIHierarchyItem item in projectNode.UIHierarchyItems)
                {
                    await CollapseAsync(item);
                }
            }
            catch (Exception ex)
            {
                AppHealth.Current.Error.TrackAsync($"{StringRes.ErrorVsGenShellCollapseSolutionItemsMessage} {ex}").FireAndForget();
            }
        }

        public override Guid GetProjectGuidByName(string projectName)
        {
            return SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                return await GetProjectGuidByNameAsync(projectName);
            });
        }

        public async Task<Guid> GetProjectGuidByNameAsync(string projectName)
        {
            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
            var project = await GetProjectByNameAsync(projectName);
            Guid projectGuid = Guid.Empty;
            try
            {
                if (project != null)
                {
                    if (ServiceProvider.GlobalProvider.GetService(typeof(SVsSolution)) is IVsSolution solution)
                    {
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
            }
            catch
            {
                projectGuid = Guid.Empty;
            }

            return projectGuid;
        }

        public override void OpenItems(params string[] itemsFullPath)
        {
            SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                await OpenItemsAsync(itemsFullPath);
            });
        }

        public async Task OpenItemsAsync(params string[] itemsFullPath)
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

        public override bool GetActiveProjectIsWts()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            bool result = false;
            var activeProjectPath = GetActiveProjectPath();
            if (!string.IsNullOrEmpty(activeProjectPath))
            {
                var metadataFileNames = new List<string>() { "Package.appxmanifest", "WTS.ProjectConfig.xml" };
                var metadataFile = metadataFileNames.FirstOrDefault(fileName => File.Exists(Path.Combine(activeProjectPath, fileName)));

                if (!string.IsNullOrEmpty(metadataFile))
                {
                    var metadataFilePath = Path.Combine(activeProjectPath, metadataFile);
                    if (File.Exists(metadataFilePath))
                    {
                        var fileContent = File.ReadAllText(metadataFilePath);
                        result = fileContent.Contains("genTemplate:Metadata");
                    }
                }
            }

            return result;
        }

        public override bool IsDebuggerEnabled()
        {
            return SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                return await IsDebuggerEnabledAsync();
            });
        }

        public async Task<bool> IsDebuggerEnabledAsync()
        {
            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
            var dte = await _dte.GetValueAsync();
            return dte.Debugger.CurrentMode != dbgDebugMode.dbgDesignMode;
        }

        public override bool IsBuildInProgress()
        {
            return SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                return await IsBuildInProgressAsync();
            });
        }

        public async Task<bool> IsBuildInProgressAsync()
        {
            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
            var dte = await _dte.GetValueAsync();
            return dte.Solution.SolutionBuild.BuildState == vsBuildState.vsBuildStateInProgress;
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

        private async Task AddReferencesToProjectsAsync(IEnumerable<ProjectReference> projectReferences)
        {
            try
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                var groupedReferences = projectReferences.GroupBy(n => n.Project, n => n);

                foreach (var project in groupedReferences)
                {
                    var parentProject = await GetProjectByPathAsync(project.Key);
                    if (project != null)
                    {
                        var proj = (VSProject)parentProject.Object;

                        foreach (var referenceToAdd in project)
                        {
                            var referenceProject = await GetProjectByPathAsync(referenceToAdd.ReferencedProject);
                            if (referenceProject != null)
                            {
                                proj.References.AddProject(referenceProject);
                            }
                        }

                        parentProject.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(StringRes.ErrorAddingReferencesBetweenProjects, ex);
            }
        }

        private async Task AddSdksForProjectAsync(string projectPath, IEnumerable<SdkReference> sdkReferences)
        {
            try
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                var project = await GetProjectByPathAsync(projectPath);
                var proj = (VSProject)project.Object;

                foreach (var referenceValue in sdkReferences)
                {
                    var refs = proj.References as VSLangProj110.References2;
                    refs.AddSDK(referenceValue.Name, referenceValue.Sdk);
                }

                project.Save();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(StringRes.ErrorAddingSdksToProject, projectPath), ex);
            }
        }

        public override void AddContextItemsToSolution(ProjectInfo projectInfo)
        {
            SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                await AddContextItemsToSolutionAsync(projectInfo);
            });
        }

        public async Task AddContextItemsToSolutionAsync(ProjectInfo projectInfo)
        {
            var filesByProject = ResolveProjectFiles(projectInfo.ProjectItems);

            var filesForExistingProjects = filesByProject.Where(k => !projectInfo.Projects.Any(p => p == k.Key));
            var nugetsForExistingProjects = projectInfo.NugetReferences.Where(n => !projectInfo.Projects.Any(p => p == n.Project)).GroupBy(n => n.Project, n => n);
            var sdksForExistingProjects = projectInfo.SdkReferences.Where(n => !projectInfo.Projects.Any(p => p == n.Project)).GroupBy(n => n.Project, n => n);

            foreach (var files in filesForExistingProjects)
            {
                if (!await IsCpsProjectAsync(files.Key))
                {
                    await AddItemsAsync(files.Key, files.Value);
                }
            }

            foreach (var nuget in nugetsForExistingProjects)
            {
                await AddNugetsForProjectAsync(nuget.Key, nuget);
            }

            foreach (var sdk in sdksForExistingProjects)
            {
                await AddSdksForProjectAsync(sdk.Key, sdk);
            }

            // Ensure projectsToAdd are ordered correctly.
            // projects from old project system should be added before project from CPS project system, as otherwise nuget restore will fail
            var orderedProject = projectInfo.Projects.OrderBy(p => IsCpsProjectAsync(p).Result);

            double secAddProjects = 0;
            double secAddFiles = 0;
            double secAddNuget = 0;

            foreach (var project in orderedProject)
            {
                var chrono = Stopwatch.StartNew();
                await AddProjectAsync(project);

                secAddProjects += chrono.Elapsed.TotalSeconds;
                chrono.Restart();

                if (!await IsCpsProjectAsync(project) && filesByProject.ContainsKey(project))
                {
                    await AddItemsAsync(project, filesByProject[project]);
                }

                secAddFiles += chrono.Elapsed.TotalSeconds;
                chrono.Restart();

                var projNugetReferences = projectInfo.NugetReferences.Where(n => n.Project == project);
                if (projNugetReferences.Any())
                {
                    await AddNugetsForProjectAsync(project, projNugetReferences);
                }

                secAddNuget += chrono.Elapsed.TotalSeconds;

                var projSdksReferences = projectInfo.SdkReferences.Where(n => n.Project == project);
                if (projSdksReferences.Any())
                {
                    await AddSdksForProjectAsync(project, projSdksReferences);
                }

                chrono.Stop();
            }

            GenContext.ToolBox.Shell.ShowStatusBarMessage(StringRes.StatusAddingProjectReferences);

            await AddReferencesToProjectsAsync(projectInfo.ProjectReferences);

            GenContext.Current.ProjectMetrics[ProjectMetricsEnum.AddProjectToSolution] = secAddProjects;
            GenContext.Current.ProjectMetrics[ProjectMetricsEnum.AddFilesToProject] = secAddFiles;
            GenContext.Current.ProjectMetrics[ProjectMetricsEnum.AddNugetToProject] = secAddNuget;
        }

        public override void ChangeSolutionConfiguration(IEnumerable<ProjectConfiguration> projectConfigurations)
        {
            SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                await ChangeSolutionConfigurationAsync(projectConfigurations);
            });
        }

        public async Task ChangeSolutionConfigurationAsync(IEnumerable<ProjectConfiguration> projectConfigurations)
        {
            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();

            var dte = await _dte.GetValueAsync();
            foreach (SolutionConfiguration solConfiguration in dte.Solution?.SolutionBuild?.SolutionConfigurations)
            {
                foreach (SolutionContext context in solConfiguration.SolutionContexts)
                {
                    var projectName = context.ProjectName;
                    if (projectConfigurations.Any(p => p.Project == projectName))
                    {
                        var projConfig = projectConfigurations.FirstOrDefault(p => p.Project == projectName);
                        context.ShouldDeploy = projConfig.SetDeploy;
                    }
                }
            }
        }

        private async Task SetActiveConfigurationAndPlatformAsync(string configurationName, string platformName, Project project)
        {
            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();

            var dte = await _dte.GetValueAsync();
            foreach (SolutionConfiguration solConfiguration in dte.Solution?.SolutionBuild?.SolutionConfigurations)
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
        }

        private async Task SetStartupProjectAsync(Project project)
        {
            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();

            if (project != null)
            {
                var solution = await GetSolutionAsync();
                solution.Properties.Item("StartupProject").Value = project.Name;
            }
        }

        private async Task<string> GetProjectTypeGuidAsync(Project project)
        {
            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
            if (project != null)
            {
                var solution = await _vssolution.GetValueAsync();
                solution.GetProjectOfUniqueName(project.FullName, out IVsHierarchy hierarchy);

                if (hierarchy is IVsAggregatableProjectCorrected aggregatableProject)
                {
                    aggregatableProject.GetAggregateProjectTypeGuids(out string projTypeGuids);

                    return projTypeGuids;
                }
            }

            return string.Empty;
        }

        private async Task<Project> GetProjectByNameAsync(string projectName)
        {
            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
            var dte = await _dte.GetValueAsync();

            foreach (var p in dte?.Solution?.Projects?.Cast<Project>())
            {
                if (p.Name == projectName)
                {
                    return p;
                }
            }

            return null;
        }

        private async Task<Project> GetProjectByProjectTypeGuidAsync(string projectTypeGuid)
        {
            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
            var dte = await _dte.GetValueAsync();

            foreach (var p in dte?.Solution?.Projects?.Cast<Project>())
            {
                if (p.Kind == projectTypeGuid)
                {
                    return p;
                }
            }

            return null;
        }

        private async Task<Project> GetActiveProjectAsync()
        {
            Project p = null;

            try
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                var dte = await _dte.GetValueAsync();

                Array projects = (Array)dte.ActiveSolutionProjects;

                if (projects?.Length >= 1)
                {
                    p = (Project)projects.GetValue(0);
                }

                return p;
            }
            catch (Exception)
            {
                // WE GET AN EXCEPTION WHEN THERE ISN'T A PROJECT LOADED
                p = null;
            }

            return p;
        }

        private async Task<Project> GetProjectByPathAsync(string projFile)
        {
            Project p = null;
            try
            {
                if (_dte != null)
                {
                    await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();

                    var vsSolution = await _vssolution.GetValueAsync();
                    vsSolution.GetProjectOfUniqueName(projFile, out IVsHierarchy hierarchy);
                    ErrorHandler.ThrowOnFailure(hierarchy.GetProperty(VSConstants.VSITEMID_ROOT, (int)__VSHPROPID.VSHPROPID_ExtObject, out object obj));
                    p = (Project)obj;
                }
            }
            catch (Exception ex)
            {
                AppHealth.Current.Error.TrackAsync(string.Format(StringRes.ErrorUnableGetProjectByPath, projFile), ex).FireAndForget();
            }

            return p;
        }

        private async Task<Solution> GetSolutionAsync()
        {
            Solution s;
            try
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                var dte = await _dte.GetValueAsync();
                return dte.Solution;
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

            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
            var dte = await _dte.GetValueAsync();

            var window = dte.Windows.Item(EnvDTE.Constants.vsWindowKindTaskList);

            window.Activate();
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

        private async Task CollapseAsync(UIHierarchyItem item)
        {
            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();

            foreach (UIHierarchyItem subitem in item.UIHierarchyItems)
            {
                await CollapseAsync(subitem);
            }

            item.UIHierarchyItems.Expanded = false;
        }

        private async Task AddNugetsForProjectAsync(string projectPath, IEnumerable<NugetReference> projectNugets)
        {
            try
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                var project = await GetProjectByPathAsync(projectPath);
                if (await IsCpsProjectAsync(projectPath))
                {
                    AddNugetToCPSProject(project, projectNugets);
                }
                else
                {
                    foreach (var reference in projectNugets)
                    {
                        var componentModel = (IComponentModel)Package.GetGlobalService(typeof(SComponentModel));
                        var installerServices = componentModel.GetService<IVsPackageInstallerServices>();

                        if (!installerServices.IsPackageInstalledEx(project, reference.PackageId, reference.Version))
                        {
                            GenContext.ToolBox.Shell.ShowStatusBarMessage(string.Format(StringRes.StatusAddingNuget, Path.GetFileName(reference.PackageId)));

                            var installer = componentModel.GetService<IVsPackageInstaller>();

                            installer.InstallPackage(null, project, reference.PackageId, reference.Version, true);
                        }
                    }

                    project.Save();
                }
            }
            catch (Exception)
            {
                WriteMissingNugetPackagesInfo(projectPath, projectNugets);
            }
        }

        private void AddNugetToCPSProject(Project project, IEnumerable<NugetReference> projectNugets)
        {
            if (project is IVsBrowseObjectContext browseObjectContext)
            {
                var threadingService = browseObjectContext.UnconfiguredProject.ProjectService.Services.ThreadingPolicy;

                threadingService.ExecuteSynchronously(
                async () =>
                {
                    var configuredProject = await browseObjectContext.UnconfiguredProject.GetSuggestedConfiguredProjectAsync().ConfigureAwait(false);

                    foreach (var reference in projectNugets)
                    {
                        GenContext.ToolBox.Shell.ShowStatusBarMessage(string.Format(StringRes.StatusAddingNuget, Path.GetFileName(reference.PackageId)));

                        await configuredProject.Services.PackageReferences.AddAsync(reference.PackageId, reference.Version);
                    }
                });
            }
        }

        private void WriteMissingNugetPackagesInfo(string projectPath, IEnumerable<NugetReference> projectNugets)
        {
            var relPath = projectPath.GetPathRelativeToDestinationParentPath();
            var sb = new StringBuilder();

            foreach (var nuget in projectNugets)
            {
                sb.AppendLine(string.Format(StringRes.ErrorMissingNugetPackagesInstallTemplate, nuget.PackageId, nuget.Version));
            }

            var missingNugetPackagesInfo = string.Format(StringRes.ErrorMissingNugetPackagesTemplate, relPath, sb.ToString());
            var fileName = Path.Combine(GenContext.Current.DestinationPath, "ERROR_NugetPackages_Missing.txt");

            File.AppendAllText(fileName, missingNugetPackagesInfo);
            GenContext.Current.FilesToOpen.Add(fileName);
        }

        private async Task<bool> IsCpsProjectAsync(string projFile)
        {
            string[] targetFrameworkTags = { "</TargetFramework>", "</TargetFrameworks>" };

            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();

            var solution = await _vssolution.GetValueAsync();
            solution.GetProjectOfUniqueName(projFile, out IVsHierarchy hierarchy);
            if (hierarchy != null)
            {
                return hierarchy.IsCapabilityMatch("CPS");
            }
            else
            {
                // Detect if project is CPS project system based
                // https://github.com/dotnet/project-system/blob/master/docs/opening-with-new-project-system.md
                return targetFrameworkTags.Any(t => File.ReadAllText(projFile).Contains(t));
            }
        }

        public override string CreateCertificate(string publisherName)
        {
            return CertificateService.Instance.CreateCertificate(publisherName);
        }

        public override VSTelemetryInfo GetVSTelemetryInfo()
        {
            return VSTelemetryService.VsTelemetryIsOptedIn();
        }

        public override void SafeTrackProjectVsTelemetry(Dictionary<string, string> properties, string pages, string features, string services, string testing, Dictionary<string, double> metrics, bool success = true)
        {
            VSTelemetryService.SafeTrackProjectVsTelemetry(properties, pages, features, metrics, success);
        }

        public override void SafeTrackNewItemVsTelemetry(Dictionary<string, string> properties, string pages, string features, string services, string testing, Dictionary<string, double> metrics, bool success = true)
        {
            VSTelemetryService.SafeTrackNewItemVsTelemetry(properties, pages, features, metrics, success);
        }

        public override void SafeTrackWizardCancelledVsTelemetry(Dictionary<string, string> properties, bool success = true)
        {
            VSTelemetryService.SafeTrackWizardCancelledVsTelemetry(properties, success);
        }
    }
}
