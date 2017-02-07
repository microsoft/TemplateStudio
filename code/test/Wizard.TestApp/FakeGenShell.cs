using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Templates.Wizard.TestApp
{
    public class FakeGenShell : GenShell
    {
        const string TEMP_TESTS_DIR = @"c:\temp\uwptemplates_tests\";

        private string _relativePath;

        public string SolutionPath { get; set; } = string.Empty;
        public TextBlock Status { get; set; }

        public FakeGenShell(string name, TextBlock status)
        {
            ProjectName = name;
            OutputPath = Path.Combine(TEMP_TESTS_DIR, name);
            SolutionPath = Path.Combine(OutputPath, $"{name}.sln");
            ProjectPath = Path.Combine(OutputPath, ProjectName);
            Status = status;
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

        public override void AddItemToActiveProject(string itemFullPath)
        {
            var projectFileName = FindProject(ProjectPath);
            if (string.IsNullOrEmpty(projectFileName))
            {
                throw new Exception($"There is not project file in {ProjectPath}");
            }
            var msbuildProj = MsBuildProject.Load(projectFileName);

            if (msbuildProj != null)
            {
                msbuildProj.AddItem(itemFullPath);
                msbuildProj.Save();
            }
        }

        public override void AddProjectToSolution(string projectFullPath)
        {
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
            Status.Text = message;
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
            dialog.ShowDialog();
        }
    }
}
