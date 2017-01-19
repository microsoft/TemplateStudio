using Microsoft.Templates.Wizard.Vs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Microsoft.VisualStudio.TemplateWizard;

namespace Microsoft.Templates.Wizard.TestApp
{
    public class TestVsData
    {
        public string ProjectFileName;
        public SolutionInfo Solution;
        public string SelectedItemPath;
    }

    public class TestVsShell : IVisualStudioShell
    {
        const string TEMP_TESTS_DIR = @"c:\temp\uwptemplates_tests\";

        private TextBlock _status;

        private TestVsData _data;        
        public TestVsData VsData { get { return _data; } }

        public TestVsShell(string fakeSolutionName, string defaultNameSpace, TextBlock statusTextBlock)
        {
            SolutionInfo solInfo = new SolutionInfo(Path.Combine(TEMP_TESTS_DIR, fakeSolutionName), fakeSolutionName, fakeSolutionName + ".sln", "Windows/Universal");
            _data = new TestVsData()
            {
                Solution = solInfo,
                ProjectFileName = string.Empty,
                SelectedItemPath = String.Empty
            };

            _status = statusTextBlock;

            if (!Directory.Exists(_data.Solution.Directory))
            {
                Directory.CreateDirectory(_data.Solution.Directory);
            }
        }

        public void UpdateSelectedItemPath(string relativePath)
        {
            _data.SelectedItemPath = relativePath;
        }

        public void AddItemToActiveProject(string itemFullPath)
        {
            var _msbuildProj = MsBuildProject.Load(_data.ProjectFileName);
            if (_msbuildProj != null)
            {
                _msbuildProj.AddItem(itemFullPath);
                _msbuildProj.Save();
            }
        }

        public void AddProjectToSolution(string projectFileName)
        {
            _data.ProjectFileName = projectFileName;
            ShowStatusBarMessage($"Adding '{projectFileName}'...");
        }

        public void CancelWizard()
        {
            throw new WizardCancelledException();
        }

        public string GetActiveProjectName()
        {
            return Path.GetFileNameWithoutExtension(_data.ProjectFileName);
        }

        public string GetActiveProjectPath()
        {
            return Path.GetDirectoryName(_data.ProjectFileName);
        }

        public string GetSolutionDirectory()
        {
            return _data.Solution.Directory;
        }

        public string GetSolutionFileName()
        {
            return _data.Solution.FileName;
        }
        public string GetSolutionFullName()
        {
            return _data.Solution.FullName;
        }
        public string GetSolutionName()
        {
            return _data.Solution.Name;
        }
        public string GetSolutionVsCategory()
        {
            return _data.Solution.TemplateCategory;
        }
        public void SaveSolution(string solutionFullPath)
        {
            StreamWriter sw = File.AppendText(solutionFullPath);
            sw.WriteLine($"Solution Saved {DateTime.Now.ToString("yyyyMMdd hh:mm:ss")}");
        }
        public void SetSolutionVsCategory(string categoryName)
        {
            //NOT REQUIRED
        }
        public void ShowStatusBarMessage(string message)
        {
            _status.Text = $"Test Shell: {message}";
        }
        public void Navigate(string url)
        {
            System.Diagnostics.Process.Start(url);
        }

        public string GetActiveProjectDefaultNamespace()
        {
            return Path.GetFileNameWithoutExtension(_data.ProjectFileName);
        }

        public string GetActiveProjectActiveDirectory(bool returnRelative)
        {
            return Path.GetDirectoryName(_data.ProjectFileName);
        }

        public string GetSelectedItemPath(bool relativeToProject)
        {
            if (relativeToProject)
            {
                return _data.SelectedItemPath;
            }
            else
            {
                return Path.Combine(_data.ProjectFileName, _data.SelectedItemPath);
            }
        }

        public string GetSelectedItemDefaultNamespace()
        {
            if (String.IsNullOrWhiteSpace(_data.SelectedItemPath))
            {
                return GetActiveProjectDefaultNamespace();
            }
            else
            {
                string pathToNamespace = GetSelectedItemPath(true).Replace(@"\", ".");
                return $"{GetActiveProjectDefaultNamespace()}.{pathToNamespace}";
            }
        }

        public void OpenItem(string fileName)
        {
            System.Diagnostics.Process.Start("notepad.exe", fileName);
        }
    }
}
