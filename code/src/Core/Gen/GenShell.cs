using Microsoft.Templates.Core.Diagnostics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Microsoft.Templates.Core.Gen
{
    public abstract class GenShell
    {
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

        public virtual void RestorePackages()
        {

        }
    }
}
