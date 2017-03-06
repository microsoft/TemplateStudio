using Microsoft.Templates.Core.Diagnostics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Microsoft.Templates.Core
{
    public abstract class GenShell
    {
        public string ProjectName { get; protected set; }
        public string ProjectPath { get; protected set; }
        public string OutputPath { get; protected set; }

        public GenShell()
        {
            ProjectName = GetActiveProjectName();
            ProjectPath = GetActiveProjectPath();
            OutputPath = GetSelectedItemPath();
        }

        public GenShell(Dictionary<string, string> replacements)
        {
            ProjectName = replacements["$safeprojectname$"];

            var di = new DirectoryInfo(replacements["$destinationdirectory$"]);
            ProjectPath = di.FullName;
            OutputPath = di.Parent.FullName;
        }

        protected abstract string GetActiveProjectName();
        protected abstract string GetActiveProjectPath();
        protected abstract string GetSelectedItemPath();

        public abstract bool SetActiveConfigurationAndPlatform(string configurationName, string platformName);
        public abstract void ShowStatusBarMessage(string message);
        public abstract void AddProjectToSolution(string projectFullPath);
        public abstract void AddItems(params string[] itemsFullPath);
        public abstract void SaveSolution(string solutionFullPath);
        public abstract string GetActiveNamespace();
        public abstract void ShowTaskList();
        public abstract void ShowModal(Window dialog);
        public abstract void CancelWizard(bool back = true);
        public abstract void WriteOutput(string data);
        public abstract void CloseSolution();
    }
}
