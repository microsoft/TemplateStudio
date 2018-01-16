// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Templates.Core.Gen;

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

        public override void AddItems(params string[] itemsFullPath)
        {
            if (itemsFullPath == null || itemsFullPath.Length == 0)
            {
                return;
            }

            var filesByProject = ResolveProjectFiles(itemsFullPath, true);

            foreach (var projectFile in filesByProject)
            {
                var msbuildProj = FakeMsBuildProject.Load(projectFile.Key);
                if (msbuildProj != null)
                {
                    foreach (var file in projectFile.Value)
                    {
                        msbuildProj.AddItem(file);
                    }

                    msbuildProj.Save();
                }
            }
        }

        public override void AddProjectToSolution(string projectFullPath)
        {
            var msbuildProj = FakeMsBuildProject.Load(projectFullPath);
            var solutionFile = FakeSolution.LoadOrCreate(_platform, SolutionPath);

            var projectRelativeToSolutionPath = projectFullPath.Replace(Path.GetDirectoryName(SolutionPath) + Path.DirectorySeparatorChar, string.Empty);

            solutionFile.AddProjectToSolution(_platform, msbuildProj.Name, msbuildProj.Guid, projectRelativeToSolutionPath);
        }

        public override string GetActiveProjectNamespace()
        {
            return GenContext.Current.ProjectName;
        }

        public override void CleanSolution()
        {
        }

        public override void SaveSolution()
        {
        }

        public override void ShowStatusBarMessage(string message)
        {
            _changeStatus?.Invoke(message);
        }

        public override string GetActiveProjectGuid()
        {
            var projectFileName = FindProject(GenContext.Current.DestinationPath);

            if (string.IsNullOrEmpty(projectFileName))
            {
                throw new Exception($"There is not project file in {GenContext.Current.DestinationPath}");
            }

            var msbuildProj = FakeMsBuildProject.Load(projectFileName);
            return msbuildProj.Guid;
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

        public override string GetSolutionPath()
        {
            return (GenContext.Current != null) ? GenContext.Current.DestinationParentPath : string.Empty;
        }

        public override string GetActiveProjectLanguage()
        {
            return _language;
        }

        protected override string GetSelectedItemPath()
        {
            return string.Empty;
        }

        private static string FindProject(string path)
        {
            return Directory.EnumerateFiles(path, "*proj", SearchOption.AllDirectories).FirstOrDefault();
        }

        public override bool SetDefaultSolutionConfiguration(string configurationName, string platformName, string projectName)
        {
            return true;
        }

        public override void ShowTaskList()
        {
        }

        public override void ShowModal(Window dialog)
        {
            dialog.Owner = _owner;
            dialog.ShowDialog();
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

        public override Guid GetVsProjectId()
        {
            Guid.TryParse(GetActiveProjectGuid(), out Guid guid);
            return guid;
        }

        public override void OpenItems(params string[] itemsFullPath)
        {
        }

        public override bool IsDebuggerEnabled()
        {
            return false;
        }
    }
}
