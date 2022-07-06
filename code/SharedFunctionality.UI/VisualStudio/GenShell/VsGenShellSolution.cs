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
using EnvDTE80;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Extensions;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Gen.Shell;
using Microsoft.Templates.Core.Helpers;
using Microsoft.Templates.SharedResources;
using Microsoft.Templates.UI.Threading;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.ProjectSystem.Properties;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell.ServiceBroker;
using Microsoft.VisualStudio.Threading;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Contracts;
using VSLangProj;
using Task = System.Threading.Tasks.Task;

namespace Microsoft.Templates.UI.VisualStudio.GenShell
{
    public class VsGenShellSolution : IGenShellSolution
    {
        private readonly VsShellService _vsShellService;

        private const string WebSiteProjectKind = "{E24C65DC-7377-472b-9ABA-BC803B73C61A}";

        public VsGenShellSolution(VsShellService vsShellService)
        {
            _vsShellService = vsShellService;
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
                    await AddItemsToProjectAsync(files.Key, files.Value);
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
                // This is a(n admittedly horrible) work-around for the issue logged at #4442
                // Adding this delay enables the deployment option to be enabled for new WinUI3 projects.
                // There isn't a filter here for only the impacted project types as that would require a bigger rewrite.
                // Hopefully, eventually, this can be removed when the underlying WinUI3 or VS issue is addressed.
                // Doing this before starting the stopwatch to avoid artificially inflating the time tracking.
                await Task.Delay(TimeSpan.FromSeconds(3));

                var chrono = Stopwatch.StartNew();

                await AddProjectToSolutionAsync(project);

                secAddProjects += chrono.Elapsed.TotalSeconds;
                chrono.Restart();

                if (!await IsCpsProjectAsync(project) && filesByProject.ContainsKey(project))
                {
                    await AddItemsToProjectAsync(project, filesByProject[project]);
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

            GenContext.ToolBox.Shell.UI.ShowStatusBarMessage(Resources.StatusAddingProjectReferences);

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

            var dte = await _vsShellService.GetDteAsync();
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
            var dte = await _vsShellService.GetDteAsync();
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
                AppHealth.Current.Error.TrackAsync($"{Resources.ErrorVsGenShellCollapseSolutionItemsMessage} {ex}").FireAndForget();
            }
        }

        private async Task CollapseSolutionItemsAsync()
        {
            try
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                var dte = await _vsShellService.GetDteAsync();
                var solutionExplorer = dte.Windows.Item(EnvDTE.Constants.vsext_wk_SProjectWindow).Object as UIHierarchy;
                var projectNode = solutionExplorer.UIHierarchyItems.Item(1)?.UIHierarchyItems.Item(1);

                foreach (UIHierarchyItem item in projectNode.UIHierarchyItems)
                {
                    await CollapseAsync(item);
                }
            }
            catch (Exception ex)
            {
                AppHealth.Current.Error.TrackAsync($"{Resources.ErrorVsGenShellCollapseSolutionItemsMessage} {ex}").FireAndForget();
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
                var startupProject = await GetProjectByProjectTypeGuidAsync(VsGenShellProperties.PackagingProjectTypeGuid);
                if (startupProject == null)
                {
                    startupProject = await GetProjectByNameAsync(projectName);
                }

                await SetActiveConfigurationAndPlatformAsync(configurationName, platformName, startupProject);
                await SetStartupProjectAsync(startupProject);
            }
            catch (Exception ex)
            {
                AppHealth.Current.Error.TrackAsync($"{Resources.ErrorUnableToSetDefaultConfiguration} {ex}").FireAndForget();
            }
        }

        private async Task<bool> IsCpsProjectAsync(string projFile)
        {
            string[] targetFrameworkTags = { "</TargetFramework>", "</TargetFrameworks>" };

            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();

            var solution = await _vsShellService.GetVsSolutionAsync();
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

        private async Task AddItemsToProjectAsync(string projPath, IEnumerable<string> projFiles)
        {
            try
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                var proj = await GetProjectByPathAsync(projPath);
                if (proj != null && proj.ProjectItems != null)
                {
                    foreach (var file in projFiles)
                    {
                        GenContext.ToolBox.Shell.UI.ShowStatusBarMessage(string.Format(Resources.StatusAddingItem, Path.GetFileName(file)));

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
                throw new Exception(string.Format(Resources.ErrorAddingItemsToProject, projPath), ex);
            }
        }

        private async Task<Project> GetProjectByPathAsync(string projFile)
        {
            var (project, _) = await GetProjectAndGuidByPathAsyncInternalAsync(projFile, getGuid: false);

            return project;
        }

        private async Task<(Project, Guid)> GetProjectAndGuidByPathAsync(string projFile)
        {
            return await GetProjectAndGuidByPathAsyncInternalAsync(projFile, getGuid: true);
        }

        private async Task<(Project, Guid)> GetProjectAndGuidByPathAsyncInternalAsync(string projFile, bool getGuid)
        {
            Project proj = null;
            Guid guid = Guid.Empty;

            try
            {
                var dte = await _vsShellService.GetDteAsync();
                if (dte != null)
                {
                    await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();

                    var vsSolution = await _vsShellService.GetVsSolutionAsync();
                    vsSolution.GetProjectOfUniqueName(projFile, out IVsHierarchy hierarchy);
                    ErrorHandler.ThrowOnFailure(hierarchy.GetProperty(VSConstants.VSITEMID_ROOT, (int)__VSHPROPID.VSHPROPID_ExtObject, out object obj));
                    proj = (Project)obj;

                    if (getGuid == true)
                    {
                        ErrorHandler.ThrowOnFailure(
                        hierarchy.GetGuidProperty(
                                    VSConstants.VSITEMID_ROOT,
                                    (int)__VSHPROPID.VSHPROPID_ProjectIDGuid,
                                    out guid));
                    }
                }
            }
            catch (Exception ex)
            {
                AppHealth.Current.Error.TrackAsync(string.Format(Resources.ErrorUnableGetProjectByPath, projFile), ex).FireAndForget();
            }

            return (proj, guid);
        }

        private async Task AddNugetsForProjectAsync(string projectPath, IEnumerable<NugetReference> packagesToAdd)
        {
            try
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                var (project, projectGuid) = await GetProjectAndGuidByPathAsync(projectPath);
                if (await IsCpsProjectAsync(projectPath))
                {
                    AddNugetToCPSProject(project, packagesToAdd);
                }
                else
                {
                    var componentModel = (IComponentModel)Package.GetGlobalService(typeof(SComponentModel));

                    var serviceContainer = (IBrokeredServiceContainer)Package.GetGlobalService(typeof(SVsBrokeredServiceContainer));
                    var serviceBroker = serviceContainer?.GetFullAccessServiceBroker();

                    var nugetService = await serviceBroker.GetProxyAsync<INuGetProjectService>(NuGetServices.NuGetProjectServiceV1);

                    using (nugetService as IDisposable)
                    {
                        var nugetPackages = await nugetService.GetInstalledPackagesAsync(projectGuid, System.Threading.CancellationToken.None);

                        IVsPackageInstaller installer = null;

                        foreach (var pkgToAdd in packagesToAdd)
                        {
                            if (!nugetPackages.Packages.Any(p => p.Id == pkgToAdd.PackageId && p.Version == pkgToAdd.Version))
                            {
                                GenContext.ToolBox.Shell.UI.ShowStatusBarMessage(string.Format(Resources.StatusAddingNuget, Path.GetFileName(pkgToAdd.PackageId)));

                                if (installer == null)
                                {
                                    installer = componentModel.GetService<IVsPackageInstaller>();
                                }

                                installer.InstallPackage(null, project, pkgToAdd.PackageId, pkgToAdd.Version, true);
                            }
                        }
                    }

                    project.Save();
                }
            }
            catch (Exception)
            {
                WriteMissingNugetPackagesInfo(projectPath, packagesToAdd);
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
                        GenContext.ToolBox.Shell.UI.ShowStatusBarMessage(string.Format(Resources.StatusAddingNuget, Path.GetFileName(reference.PackageId)));

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
                sb.AppendLine(string.Format(Resources.ErrorMissingNugetPackagesInstallTemplate, nuget.PackageId, nuget.Version));
            }

            var missingNugetPackagesInfo = string.Format(Resources.ErrorMissingNugetPackagesTemplate, relPath, sb);
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
                throw new Exception(string.Format(Resources.ErrorAddingSdksToProject, projectPath), ex);
            }
        }

        private async Task AddProjectToSolutionAsync(string project)
        {
            var dte = await _vsShellService.GetDteAsync();

            try
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                GenContext.ToolBox.Shell.UI.ShowStatusBarMessage(string.Format(Resources.StatusAddingProject, Path.GetFileName(project)));

                dte.Solution.AddFromFile(project);
            }
            catch (Exception ex)
            {
                // If adding to an existing solution,
                // the `IgnoreThis` project may have already been added by VS.
                // We don't want an error if that's the case.
                bool projectAlreadyExistsInSolution = false;
                var item = dte.Solution.Projects.GetEnumerator();

                while (item.MoveNext() && !projectAlreadyExistsInSolution)
                {
                    // skip if Current item is not a project or if the project Kind is a Web Site project (as won't have a project file to load)
                    if (!(item.Current is Project proj) || proj.Kind == WebSiteProjectKind)
                    {
                        continue;
                    }

                    if (proj.Kind == ProjectKinds.vsProjectKindSolutionFolder)
                    {
                        projectAlreadyExistsInSolution =
                            GetSolutionFolderProjects(proj).Contains(project);
                    }
                    else
                    {
                        // Projects that are not loaded with the solution won't provide access to their properties (including the FileName)
                        if (proj.Properties != null)
                        {
                            if (proj.FileName == project)
                            {
                                projectAlreadyExistsInSolution = true;
                            }
                        }
                    }
                }

                if (!projectAlreadyExistsInSolution)
                {
                    throw new Exception(string.Format(Resources.ErrorAddingProject, project), ex);
                }
            }
        }

        private static IEnumerable<string> GetSolutionFolderProjects(Project solutionFolder)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var result = new List<string>();

            for (var i = 1; i <= solutionFolder.ProjectItems.Count; i++)
            {
                var subProject = solutionFolder.ProjectItems.Item(i).SubProject;

                if (subProject == null)
                {
                    continue;
                }

                // If this is another solution folder, do a recursive call, otherwise add
                if (subProject.Kind == ProjectKinds.vsProjectKindSolutionFolder)
                {
                    result.AddRange(GetSolutionFolderProjects(subProject));
                }
                else if (subProject is Project proj
                    && proj.Kind != WebSiteProjectKind)
                {
                    result.Add(subProject.FileName);
                }
            }

            return result;
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
                throw new Exception(Resources.ErrorAddingReferencesBetweenProjects, ex);
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
            var dte = await _vsShellService.GetDteAsync();

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

            var dte = await _vsShellService.GetDteAsync();
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
                var dte = await _vsShellService.GetDteAsync();
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
            var dte = await _vsShellService.GetDteAsync();

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
