// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Extensions;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Gen.Shell;
using Microsoft.Templates.Core.Helpers;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.Threading;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.ProjectSystem.Properties;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using NuGet.VisualStudio;
using VSLangProj;
using Task = System.Threading.Tasks.Task;

namespace Microsoft.Templates.UI.VisualStudio.GenShell
{
    public class VsGenShellSolution : IGenShellSolution
    {
        private readonly string _packagingProjectTypeGuid;
        private readonly AsyncLazy<IVsSolution> _vssolution;
        private readonly AsyncLazy<DTE> _dte;

        public VsGenShellSolution(AsyncLazy<IVsSolution> vssolution, AsyncLazy<DTE> dte, string packagingProjectTypeGuid)
        {
            _vssolution = vssolution;
            _dte = dte;
            _packagingProjectTypeGuid = packagingProjectTypeGuid;
        }

        public void AddContextItemsToSolution(ProjectInfo projectInfo)
        {
            SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                await AddContextItemsToSolutionAsync(projectInfo);
            });
        }

        private async Task AddContextItemsToSolutionAsync(ProjectInfo projectInfo)
        {
            var filesByProject = ProjectHelper.ResolveProjectFiles(projectInfo.ProjectItems);

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

            GenContext.ToolBox.Shell.UI.ShowStatusBarMessage(StringRes.StatusAddingProjectReferences);

            await AddReferencesToProjectsAsync(projectInfo.ProjectReferences);

            GenContext.Current.ProjectMetrics[ProjectMetricsEnum.AddProjectToSolution] = secAddProjects;
            GenContext.Current.ProjectMetrics[ProjectMetricsEnum.AddFilesToProject] = secAddFiles;
            GenContext.Current.ProjectMetrics[ProjectMetricsEnum.AddNugetToProject] = secAddNuget;
        }

        public void ChangeSolutionConfiguration(IEnumerable<ProjectConfiguration> projectConfigurations)
        {
            SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                await ChangeSolutionConfigurationAsync(projectConfigurations);
            });
        }

        private async Task ChangeSolutionConfigurationAsync(IEnumerable<ProjectConfiguration> projectConfigurations)
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

        public void CloseSolution()
        {
            SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                await CloseSolutionAsync();
            });
        }

        private async Task CloseSolutionAsync()
        {
            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
            var dte = await _dte.GetValueAsync();
            dte.Solution.Close();
        }

        public void CollapseSolutionItems()
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

        private async Task CollapseSolutionItemsAsync()
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

        public void SetDefaultSolutionConfiguration(string configurationName, string platformName, string projectName)
        {
            SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                await SetDefaultSolutionConfigurationAsync(configurationName, platformName, projectName);
            });
        }

        private async Task SetDefaultSolutionConfigurationAsync(string configurationName, string platformName, string projectName)
        {
            try
            {
                var startupProject = await GetProjectByProjectTypeGuidAsync(_packagingProjectTypeGuid);
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
                        GenContext.ToolBox.Shell.UI.ShowStatusBarMessage(string.Format(StringRes.StatusAddingItem, Path.GetFileName(file)));

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
                            GenContext.ToolBox.Shell.UI.ShowStatusBarMessage(string.Format(StringRes.StatusAddingNuget, Path.GetFileName(reference.PackageId)));

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
                        GenContext.ToolBox.Shell.UI.ShowStatusBarMessage(string.Format(StringRes.StatusAddingNuget, Path.GetFileName(reference.PackageId)));

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

            var missingNugetPackagesInfo = string.Format(StringRes.ErrorMissingNugetPackagesTemplate, relPath, sb);
            var fileName = Path.Combine(GenContext.Current.DestinationPath, "ERROR_NugetPackages_Missing.txt");

            File.AppendAllText(fileName, missingNugetPackagesInfo);
            GenContext.Current.FilesToOpen.Add(fileName);
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

        private async Task AddProjectAsync(string project)
        {
            try
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                GenContext.ToolBox.Shell.UI.ShowStatusBarMessage(string.Format(StringRes.StatusAddingProject, Path.GetFileName(project)));

                var dte = await _dte.GetValueAsync();
                dte.Solution.AddFromFile(project);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(StringRes.ErrorAddingProject, project), ex);
            }
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

        private async Task CollapseAsync(UIHierarchyItem item)
        {
            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();

            foreach (UIHierarchyItem subitem in item.UIHierarchyItems)
            {
                await CollapseAsync(subitem);
            }

            item.UIHierarchyItems.Expanded = false;
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
    }
}
