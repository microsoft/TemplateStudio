using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Microsoft.Templates.Wizard.TestApp
{
    public class TestGenShell : GenShell
    {
        const string TEMP_TESTS_DIR = @"c:\temp\uwptemplates_tests\";

        private string _projectPath;
        private string _relativePath;

        public string SolutionPath { get; set; } = string.Empty;
        public TextBlock Status { get; set; }

        public TestGenShell(string name, TextBlock status)
        {
            Name = name;
            OutputPath = Path.Combine(TEMP_TESTS_DIR, name);
            SolutionPath = Path.Combine(OutputPath, $"{name}.sln");
            Status = status;
        }

        public void UpdateRelativePath(string relative)
        {
            if (string.IsNullOrEmpty(_projectPath))
            {
                return;
            }
            _relativePath = relative;
            if (string.IsNullOrEmpty(relative))
            {
                OutputPath = Path.GetDirectoryName(_projectPath);
            }
            else
            {
                Path.Combine(Path.GetDirectoryName(_projectPath), relative);
            }
        }

        public override void AddItemToActiveProject(string itemFullPath)
        {
            var msbuildProj = MsBuildProject.Load(_projectPath);

            if (msbuildProj != null)
            {
                msbuildProj.AddItem(itemFullPath);
                msbuildProj.Save();
            }
        }

        public override void AddProjectToSolution(string projectFullPath)
        {
            _projectPath = projectFullPath;
        }

        public override string GetActiveNamespace()
        {
            var relativeNs = string.IsNullOrEmpty(_relativePath) ? string.Empty : _relativePath.Replace(@"\", ".");
            if (string.IsNullOrEmpty(relativeNs))
            {
                return Name;
            }
            else
            {
                return $"{Name}.{relativeNs}";
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
            return Name;
        }

        protected override string GetActiveProjectPath()
        {
            return string.Empty;

            //if (string.IsNullOrEmpty(ProjectPath))
            //{
            //    return OutputPath;
            //}
            //else
            //{
            //    return ProjectPath;
            //}
        }

        protected override string GetSelectedItemPath()
        {
            return string.Empty;

            //if (string.IsNullOrEmpty(RelativePath))
            //{
            //    return GetActiveProjectPath();
            //}
            //else
            //{
            //    return Path.Combine(GetActiveProjectPath(), RelativePath);
            //}
        }
    }
}
