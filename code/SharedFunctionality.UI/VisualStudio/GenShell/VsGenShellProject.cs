﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen.Shell;
using Microsoft.Templates.UI.Threading;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Flavor;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.Templates.UI.VisualStudio.GenShell
{
    public class VsGenShellProject : IGenShellProject
    {
        private readonly VsShellService _vsShellService;
        private const string UwpProjectTypeGuid = "A5A43C5B-DE2A-4C0C-9213-0A381AF9435A";

        public VsGenShellProject(VsShellService vsShellService)
        {
            _vsShellService = vsShellService;
        }

        public bool IsActiveProjectWpf()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            return ActiveProjectHasCapability("WPF");
        }

        public bool IsActiveProjectWinUI()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            return ActiveProjectHasCapability("WINUI");
        }

        public bool IsActiveProjectUwp()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            return ActiveProjectIsOfType(UwpProjectTypeGuid);
        }

        private bool ActiveProjectHasCapability(string capability)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var proj = GetActiveProject();
            if (proj != null)
            {
                var vsProject = ToVsHierarchy(proj);
                if (vsProject != null)
                {
                    return vsProject.IsCapabilityMatch(capability);
                }
            }

            return false;
        }

        private bool ActiveProjectIsOfType(string guidTypeString)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            try
            {
                var proj = GetActiveProject();
                if (proj != null)
                {
                    var vsHierarchy = ToVsHierarchy(proj);

                    var aggregatableProject = (IVsAggregatableProject)vsHierarchy;

                    if (aggregatableProject.GetAggregateProjectTypeGuids(out string projectTypeGuids) == VSConstants.S_OK)
                    {
                        return projectTypeGuids.Contains(guidTypeString);
                    }
                }
            }
            catch
            {
                // Not all project types can be cast as an IVsAggregatableProject
                // Ignore any issues here.
            }

            return false;
        }

        public IVsHierarchy ToVsHierarchy(EnvDTE.Project project)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var solution = ServiceProvider.GlobalProvider.GetService(typeof(IVsSolution)) as IVsSolution;

            if (solution != null)
            {
                _ = solution.GetProjectOfUniqueName(project.UniqueName, out IVsHierarchy hierarchy);

                return hierarchy;
            }

            return null;
        }

        public bool GetActiveProjectIsWts()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            bool result = false;
            var activeProjectPath = GetActiveProjectPath();
            var projectKind = GetActiveProjectKind();

            if (!string.IsNullOrEmpty(activeProjectPath) && projectKind != VsGenShellProperties.PackagingProjectTypeGuid)
            {
                var metadataFileNames = new List<string>() { "Package.appxmanifest", "TemplateStudio.xml" };
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

        public string GetActiveProjectLanguage()
        {
            return SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                return await GetActiveProjectLanguageAsync();
            });
        }

        private async Task<string> GetActiveProjectLanguageAsync()
        {
            var p = await GetActiveProjectAsync();

            if (p != null)
            {
                switch (Path.GetExtension(await SafeGetFullNameAsync(p)))
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

        public string GetActiveProjectName()
        {
            return SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                return await GetActiveProjectNameAsync();
            });
        }

        private async Task<string> GetActiveProjectNameAsync()
        {
            var p = await GetActiveProjectAsync();

            return p?.Name;
        }

        public string GetActiveProjectNamespace()
        {
            return SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                return await GetActiveProjectNamespaceAsync();
            });
        }

        private async Task<string> GetActiveProjectNamespaceAsync()
        {
            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
            var p = await GetActiveProjectAsync();

            if (p != null && p.Properties != null)
            {
                return p.Properties.Item("RootNamespace")?.Value?.ToString();
            }

            return null;
        }

        public string GetActiveProjectPath()
        {
            return SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                return await GetActiveProjectPathAsync();
            });
        }

        private async Task<string> GetActiveProjectPathAsync()
        {
            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
            try
            {
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
            catch (Exception exc)
            {
                // Most liekly to be a project type that throws a NRE when accessing FileName
                System.Diagnostics.Debug.WriteLine(exc);
                return null;
            }
        }

        public string GetActiveProjectTypeGuids()
        {
            return SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                return await GetActiveProjectTypeGuidsAsync();
            });
        }

        private async Task<string> GetActiveProjectTypeGuidsAsync()
        {
            var project = await GetActiveProjectAsync();
            return await GetProjectTypeGuidAsync(project);
        }

        public Guid GetProjectGuidByName(string projectName)
        {
            return SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                return await GetProjectGuidByNameAsync(projectName);
            });
        }

        private async Task<Guid> GetProjectGuidByNameAsync(string projectName)
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

        private string GetActiveProjectKind()
        {
            return SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                return await GetActiveProjectKindAsync();
            });
        }

        private async Task<string> GetActiveProjectKindAsync()
        {
            var p = await GetActiveProjectAsync();

            return p?.Kind;
        }

        private Project GetActiveProject()
        {
            return SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                return await GetActiveProjectAsync();
            });
        }

        private async Task<Project> GetActiveProjectAsync()
        {
            Project p = null;

            try
            {
                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                var dte = await _vsShellService.GetDteAsync();

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

        private async Task<string> SafeGetFullNameAsync(Project project)
        {
            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
            if (project != null)
            {
                return project.FullName ?? string.Empty;
            }

            return string.Empty;
        }

        private async Task<string> GetProjectTypeGuidAsync(Project project)
        {
            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
            if (project != null)
            {
                var solution = await _vsShellService.GetVsSolutionAsync();
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
