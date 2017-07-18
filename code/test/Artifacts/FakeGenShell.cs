// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.IO;
using System.Linq;
using System.Windows;

using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Test.Artifacts.MSBuild;
using Microsoft.VisualStudio.TemplateWizard;

namespace Microsoft.Templates.Test.Artifacts
{
    public class FakeGenShell : GenShell
    {
        private readonly string _language;
        private readonly Action<string> _changeStatus;
        private readonly Action<string> _addLog;
        private readonly Window _owner;

        public string SolutionPath
        {
            get
            {
                if (GenContext.Current != null)
                {
                    return Path.Combine(Path.GetDirectoryName(GenContext.Current.ProjectPath), $"{GenContext.Current.ProjectName}.sln");
                }
                return null;
            }
        }

        public FakeGenShell(string language, Action<string> changeStatus = null, Action<string> addLog = null, Window owner = null)
        {
            _language = language;
            _changeStatus = changeStatus;
            _addLog = addLog;
            _owner = owner;
        }

        public override void AddItems(params string[] itemsFullPath)
        {
            if (itemsFullPath == null || itemsFullPath.Length == 0)
            {
                return;
            }

            var projectFileName = FindProject(GenContext.Current.ProjectPath);

            if (string.IsNullOrEmpty(projectFileName))
            {
                throw new Exception($"There is not project file in {GenContext.Current.ProjectPath}");
            }

            var msbuildProj = MsBuildProject.Load(projectFileName);

            if (msbuildProj != null)
            {
                foreach (var item in itemsFullPath)
                {
                    msbuildProj.AddItem(item);
                }

                msbuildProj.Save();
            }
        }

        public override void AddProjectToSolution(string projectFullPath)
        {
            var msbuildProj = MsBuildProject.Load(projectFullPath);
            var solutionFile = MSBuildSolution.Create(SolutionPath);

            solutionFile.AddProjectToSolution(msbuildProj.Name, msbuildProj.Guid, projectFullPath.EndsWith(".csproj", StringComparison.InvariantCultureIgnoreCase));
        }

        public override string GetActiveProjectNamespace()
        {
            return GenContext.Current.ProjectName;
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
            var projectFileName = FindProject(GenContext.Current.ProjectPath);

            if (string.IsNullOrEmpty(projectFileName))
            {
                throw new Exception($"There is not project file in {GenContext.Current.ProjectPath}");
            }

            var msbuildProj = MsBuildProject.Load(projectFileName);
            return msbuildProj.Guid;
        }

        public override string GetActiveProjectName()
        {
            return GenContext.Current.ProjectName;
        }

        public override string GetActiveProjectPath()
        {
            return (GenContext.Current != null) ? GenContext.Current.ProjectPath : string.Empty;
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
            return Directory.EnumerateFiles(path, "*proj").FirstOrDefault();
        }

        public override bool SetActiveConfigurationAndPlatform(string configurationName, string platformName)
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
            return Guid.Empty;
        }

        public override void OpenItems(params string[] itemsFullPath)
        {
        }
    }
}

