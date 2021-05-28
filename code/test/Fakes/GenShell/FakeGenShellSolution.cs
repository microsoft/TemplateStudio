// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Gen.Shell;
using Microsoft.Templates.Core.Helpers;

namespace Microsoft.Templates.Fakes.GenShell
{
    public class FakeGenShellSolution : IGenShellSolution
    {
        private readonly string _solutionPath;

        public string Language { get; set; }

        public string Platform { get; set; }

        public string AppModel { get; set; }

        public FakeGenShellSolution(string platform, string language, string appModel, string solutionPath)
        {
            Platform = platform;
            Language = language;
            AppModel = appModel;
            _solutionPath = solutionPath;
        }

        public void AddContextItemsToSolution(ProjectInfo projectInfo)
        {
            var filesByProject = ProjectHelper.ResolveProjectFiles(projectInfo.ProjectItems);

            var filesForExistingProjects = filesByProject.Where(k => !projectInfo.Projects.Any(p => p == k.Key));
            var nugetsForExistingProjects = projectInfo.NugetReferences.Where(n => !projectInfo.Projects.Any(p => p == n.Project)).GroupBy(n => n.Project, n => n);
            var sdksForExistingProjects = projectInfo.SdkReferences.Where(n => !projectInfo.Projects.Any(p => p == n.Project)).GroupBy(n => n.Project, n => n);

            foreach (var files in filesForExistingProjects)
            {
                if (!IsCpsProject(files.Key))
                {
                    AddItems(files.Key, files.Value);
                }
            }

            foreach (var project in nugetsForExistingProjects)
            {
                AddNugetsForProject(project.Key, project);
            }

            foreach (var sdk in sdksForExistingProjects)
            {
                AddSdksForProject(sdk.Key, sdk);
            }

            foreach (var project in projectInfo.Projects)
            {
                var msbuildProj = FakeMsBuildProject.Load(project);
                var solutionFile = FakeSolution.LoadOrCreate(Platform, AppModel, Language, _solutionPath);

                var projectRelativeToSolutionPath = project.Replace(Path.GetDirectoryName(_solutionPath) + Path.DirectorySeparatorChar, string.Empty);

                var projGuid = !string.IsNullOrEmpty(msbuildProj.Guid) ? msbuildProj.Guid : Guid.NewGuid().ToString();

                solutionFile.AddProjectToSolution(Platform, AppModel, Language, msbuildProj.Name, projGuid, projectRelativeToSolutionPath, IsCpsProject(project), HasPlatforms(project));

                if (!IsCpsProject(project) && filesByProject.ContainsKey(project))
                {
                    AddItems(project, filesByProject[project]);
                }

                AddNugetsForProject(project, projectInfo.NugetReferences.Where(n => n.Project == project));
                AddSdksForProject(project, projectInfo.SdkReferences.Where(n => n.Project == project));
            }

            AddReferencesToProjects(projectInfo.ProjectReferences);
        }

        public void ChangeSolutionConfiguration(IEnumerable<ProjectConfiguration> projectConfiguration)
        {
        }

        public void CloseSolution()
        {
        }

        public void CollapseSolutionItems()
        {
        }

        public void SetDefaultSolutionConfiguration(string configurationName, string platformName, string projectName)
        {
        }

        private bool IsCpsProject(string projectPath)
        {
            string[] targetFrameworkTags = { "</TargetFramework>", "</TargetFrameworks>" };
            return targetFrameworkTags.Any(t => File.ReadAllText(projectPath).Contains(t));
        }

        private void AddItems(string projectPath, IEnumerable<string> filesToAdd)
        {
            var msbuildProj = FakeMsBuildProject.Load(projectPath);
            if (msbuildProj != null)
            {
                foreach (var file in filesToAdd)
                {
                    msbuildProj.AddItem(file);
                }

                msbuildProj.Save();
            }
        }

        private void AddNugetsForProject(string projectPath, IEnumerable<NugetReference> nugetReferences)
        {
            if (nugetReferences.Any())
            {
                if (Language == ProgrammingLanguages.Cpp)
                {
                    var packagesConfig = FakePackagesConfig.Load(Path.Combine(Path.GetDirectoryName(projectPath), "packages.config"));
                    var msbuildProj = FakeMsBuildProject.Load(projectPath);

                    foreach (var nugetPackages in nugetReferences)
                    {
                        packagesConfig.AddNugetReference(nugetPackages);
                        msbuildProj.AddNugetImport(nugetPackages);
                    }

                    packagesConfig.Save();
                    msbuildProj.Save();
                }
                else
                {
                    var msbuildProj = FakeMsBuildProject.Load(projectPath);

                    foreach (var nugetPackages in nugetReferences)
                    {
                        msbuildProj.AddNugetReference(nugetPackages);
                    }

                    msbuildProj.Save();
                }
            }
        }

        private void AddSdksForProject(string projectPath, IEnumerable<SdkReference> sdkReferences)
        {
            var project = FakeMsBuildProject.Load(projectPath);

            foreach (var referenceValue in sdkReferences)
            {
                project.AddSDKReference(referenceValue);
            }

            project.Save();
        }

        private void AddReferencesToProjects(IEnumerable<ProjectReference> projectReferences)
        {
            var solution = FakeSolution.LoadOrCreate(Platform, AppModel, Language, _solutionPath);
            var projectGuids = solution.GetProjectGuids();

            var groupedReferences = projectReferences.GroupBy(n => n.Project, n => n);

            foreach (var project in groupedReferences)
            {
                var parentProject = FakeMsBuildProject.Load(project.Key);

                foreach (var referenceToAdd in project)
                {
                    var referenceProject = FakeMsBuildProject.Load(referenceToAdd.ReferencedProject);

                    var name = referenceProject.Name;
                    var guid = projectGuids[name];
                    if (guid == "{}")
                    {
                        guid = "{" + Guid.NewGuid().ToString() + "}";
                    }

                    parentProject.AddProjectReference(referenceToAdd.ReferencedProject, guid, name);
                }

                parentProject.Save();
            }
        }

        private bool HasPlatforms(string projectPath)
        {
            return File.ReadAllText(projectPath).Contains("</Platforms>");
        }
    }
}
