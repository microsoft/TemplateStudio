// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Utilities.Services;
using Microsoft.VisualStudio.TemplateWizard;

namespace Microsoft.Templates.Fakes
{
    public class FakeGenShell : GenShell
    {
        private readonly Action<string> _changeStatus;
        private readonly Action<string> _addLog;
        private readonly Window _owner;

        private string _language;
        private string _platform;
        private string _appModel;

        public string SolutionPath
        {
            get
            {
                if (GenContext.Current != null)
                {
                    return Path.Combine(Path.GetDirectoryName(GenContext.Current.DestinationPath), $"{GenContext.Current.ProjectName}.sln");
                }

                return null;
            }
        }

        public FakeGenShell(string platform, string language, Action<string> changeStatus = null, Action<string> addLog = null, Window owner = null)
        {
            _platform = platform;
            _appModel = string.Empty;
            _language = language;
            _changeStatus = changeStatus;
            _addLog = addLog;
            _owner = owner;
        }

        public void SetCurrentLanguage(string language)
        {
            _language = language;
        }

        public void SetCurrentPlatform(string platform)
        {
            _platform = platform;
        }

        public void SetCurrentAppModel(string appModel)
        {
            _appModel = appModel;
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

        public override void AddContextItemsToSolution(ProjectInfo projectInfo)
        {
            var filesByProject = ResolveProjectFiles(projectInfo.ProjectItems);

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
                var solutionFile = FakeSolution.LoadOrCreate(_platform, _language, SolutionPath);

                var projectRelativeToSolutionPath = project.Replace(Path.GetDirectoryName(SolutionPath) + Path.DirectorySeparatorChar, string.Empty);

                var projGuid = !string.IsNullOrEmpty(msbuildProj.Guid) ? msbuildProj.Guid : Guid.NewGuid().ToString();

                solutionFile.AddProjectToSolution(_platform, _appModel, _language, msbuildProj.Name, projGuid, projectRelativeToSolutionPath, IsCpsProject(project), HasPlatforms(project));

                if (!IsCpsProject(project) && filesByProject.ContainsKey(project))
                {
                    AddItems(project, filesByProject[project]);
                }

                AddNugetsForProject(project, projectInfo.NugetReferences.Where(n => n.Project == project));
                AddSdksForProject(project, projectInfo.SdkReferences.Where(n => n.Project == project));
            }

            AddReferencesToProjects(projectInfo.ProjectReferences);
        }

        private void AddNugetsForProject(string projectPath, IEnumerable<NugetReference> nugetReferences)
        {
            if (nugetReferences.Any())
            {
                if (_language == ProgrammingLanguages.Cpp)
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

        public override string GetActiveProjectNamespace()
        {
            return GenContext.Current.ProjectName;
        }

        public override void ShowStatusBarMessage(string message)
        {
            _changeStatus?.Invoke(message);
        }

        public override string GetActiveProjectTypeGuids()
        {
            var projectFileName = FindProject(GenContext.Current.DestinationPath);

            if (string.IsNullOrEmpty(projectFileName))
            {
                throw new Exception($"There is not project file in {GenContext.Current.DestinationPath}");
            }

            var msbuildProj = FakeMsBuildProject.Load(projectFileName);
            return msbuildProj.ProjectTypeGuids;
        }

        public override string GetActiveProjectName()
        {
            return GenContext.Current.ProjectName;
        }

        public override string GetActiveProjectPath()
        {
            return (GenContext.Current != null) ? GenContext.Current.DestinationPath : string.Empty;
        }

        public override string GetActiveProjectLanguage()
        {
            return _language;
        }

        private static string FindProject(string path)
        {
            return Directory.EnumerateFiles(path, "*proj", SearchOption.AllDirectories).FirstOrDefault();
        }

        public override void ChangeSolutionConfiguration(IEnumerable<ProjectConfiguration> projectConfiguration)
        {
        }

        public override void SetDefaultSolutionConfiguration(string configurationName, string platformName, string projectName)
        {
        }

        public override void ShowTaskList()
        {
        }

        public override void ShowModal(IWindow shell)
        {
            if (shell is Window dialog)
            {
                dialog.Owner = _owner;
                dialog.ShowDialog();
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

        public override void WriteOutput(string data)
        {
            _addLog?.Invoke(data);
        }

        public override void CloseSolution()
        {
        }

        public override Guid GetProjectGuidByName(string projectName)
        {
            var projectFileName = FindProject(GenContext.Current.DestinationPath);
            var msbuildProj = FakeMsBuildProject.Load(projectFileName);
            var guid = msbuildProj.Guid;
            if (string.IsNullOrEmpty(guid))
            {
                var solution = FakeSolution.LoadOrCreate(_platform, _language, SolutionPath);
                guid = solution.GetProjectGuids().First(p => p.Key == projectName).Value;
            }

            Guid.TryParse(guid, out Guid parsedGuid);
            return parsedGuid;
        }

        public override void OpenItems(params string[] itemsFullPath)
        {
        }

        public override bool IsDebuggerEnabled()
        {
            return false;
        }

        public override bool IsBuildInProgress()
        {
            return false;
        }

        public override void OpenProjectOverview()
        {
        }

        private void AddReferencesToProjects(IEnumerable<ProjectReference> projectReferences)
        {
            var solution = FakeSolution.LoadOrCreate(_platform, _language, SolutionPath);
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

        private void AddSdksForProject(string projectPath, IEnumerable<SdkReference> sdkReferences)
        {
            var project = FakeMsBuildProject.Load(projectPath);

            foreach (var referenceValue in sdkReferences)
            {
                project.AddSDKReference(referenceValue);
            }

            project.Save();
        }

        private bool IsCpsProject(string projectPath)
        {
            string[] targetFrameworkTags = { "</TargetFramework>", "</TargetFrameworks>" };
            return targetFrameworkTags.Any(t => File.ReadAllText(projectPath).Contains(t));
        }

        private bool HasPlatforms(string projectPath)
        {
            return File.ReadAllText(projectPath).Contains("</Platforms>");
        }

        public override string CreateCertificate(string publisherName)
        {
            return CertificateService.Instance.CreateCertificate(publisherName);
        }

        public override VSTelemetryInfo GetVSTelemetryInfo()
        {
            return new VSTelemetryInfo()
            {
                OptedIn = true,
                VisualStudioCulture = string.Empty,
                VisualStudioEdition = string.Empty,
                VisualStudioExeVersion = string.Empty,
                VisualStudioManifestId = string.Empty,
            };
        }

        public override void SafeTrackProjectVsTelemetry(Dictionary<string, string> properties, string pages, string features, string services, string testing, Dictionary<string, double> metrics, bool success = true)
        {
        }

        public override void SafeTrackNewItemVsTelemetry(Dictionary<string, string> properties, string pages, string features, string services, string testing, Dictionary<string, double> metrics, bool success = true)
        {
        }

        public override void SafeTrackWizardCancelledVsTelemetry(Dictionary<string, string> properties, bool success = true)
        {
        }

        public override bool GetActiveProjectIsWts()
        {
            return true;
        }

        public override bool IsSdkInstalled(string name)
        {
            return true;
        }
    }
}
