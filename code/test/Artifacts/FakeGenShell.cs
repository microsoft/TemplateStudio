using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Test.Artifacts.MSBuild;
using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Templates.Test.Artifacts
{
    public class FakeGenShell : GenShell
    {
        private readonly Action<string> _changeStatus;
        private readonly Window _owner;

        public string SolutionPath
        {
            get
            {
                if (GenContext.Current != null)
                {
                    return Path.Combine(Path.GetDirectoryName(GenContext.Current.OutputPath), $"{GenContext.Current.ProjectName}.sln");
                }
                throw new Exception("Context doesn't exists");
            }
        }

        public FakeGenShell(Action<string> changeStatus = null, Window owner = null)
        {
            _changeStatus = changeStatus;
            _owner = owner;
        }

        public override void AddItems(params string[] itemsFullPath)
        {
            if (itemsFullPath == null || itemsFullPath.Length == 0)
            {
                return;
            }

            var projectFileName = FindProject(GenContext.Current.OutputPath);
            if (string.IsNullOrEmpty(projectFileName))
            {
                throw new Exception($"There is not project file in {GenContext.Current.OutputPath}");
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
            solutionFile.AddProjectToSolution(msbuildProj.Name, msbuildProj.Guid);

        }

        public override string GetActiveNamespace()
        {
            return GenContext.Current.ProjectName;
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
            return GenContext.Current.ProjectName;
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
            Console.Out.WriteLine(data);
        }

        public override void CloseSolution()
        {
        }
    }
}

