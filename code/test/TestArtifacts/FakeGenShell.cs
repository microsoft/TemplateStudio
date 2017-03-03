using Microsoft.Templates.Core;
using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TestArtifacts
{
    public class FakeGenShell : GenShell
    {
        private string _relativePath;
        private readonly Action<string> _changeStatus;
        private readonly Window _owner;

        public string SolutionPath { get; set; } = string.Empty;

        public FakeGenShell(string name, string location, string solutionName = null, Action<string> changeStatus = null, Window owner = null)
        {
            ProjectName = name;
            OutputPath = Path.Combine(location, name);
            SolutionPath = Path.Combine(OutputPath, $"{name}.sln");
            ProjectPath = Path.Combine(OutputPath, ProjectName);

            _changeStatus = changeStatus;
            _owner = owner;
        }

        public void UpdateRelativePath(string relative)
        {
            _relativePath = relative;

            if (string.IsNullOrEmpty(relative))
            {
                OutputPath = Path.GetDirectoryName(ProjectPath);
            }
            else
            {
                OutputPath = Path.Combine(Path.GetDirectoryName(ProjectPath), relative);
            }
        }

        public override void AddItems(params string[] itemsFullPath)
        {
            if (itemsFullPath == null || itemsFullPath.Length == 0)
            {
                return;
            }

            var projectFileName = FindProject(ProjectPath);
            if (string.IsNullOrEmpty(projectFileName))
            {
                throw new Exception($"There is not project file in {ProjectPath}");
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

            var solutionFile = SolutionFile.Load(SolutionPath);
            solutionFile.AddProjectToSolution(msbuildProj.Name, msbuildProj.Guid);

        }

        public override string GetActiveNamespace()
        {
            var relativeNs = string.IsNullOrEmpty(_relativePath) ? string.Empty : _relativePath.Replace(@"\", ".");
            if (string.IsNullOrEmpty(relativeNs))
            {
                return ProjectName;
            }
            else
            {
                return $"{ProjectName}.{relativeNs}";
            }
        }

        public override void SaveSolution(string solutionFullPath)
        {
        }

        public override void ShowStatusBarMessage(string message)
        {
            _changeStatus?.Invoke(message);
        }

        protected override string GetActiveProjectName()
        {
            return ProjectName;
        }

        protected override string GetActiveProjectPath()
        {
            return string.Empty;
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

        public override void CancelWizard()
        {
            throw new WizardBackoutException();
        }

        public override void WriteOutput(string data)
        {
            Console.Out.WriteLine(data);
        }
    }
}

