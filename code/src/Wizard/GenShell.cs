using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Wizard
{
    public abstract class GenShell
    {
        public string Name { get; set; }
        public string OutputPath { get; set; }

        public GenShell()
        {
            Name = GetActiveProjectName();
            OutputPath = GetSelectedItemPath();
        }

        public GenShell(Dictionary<string, string> replacements)
        {
            Name = replacements["$safeprojectname$"];

            var di = new DirectoryInfo(replacements["$destinationdirectory$"]);
            OutputPath = di.Parent.FullName;
        }

        protected abstract string GetActiveProjectName();
        protected abstract string GetActiveProjectPath();
        protected abstract string GetSelectedItemPath();

        public abstract bool SetActiveConfigurationAndPlatform(string configurationName, string platformName);
        public abstract void ShowStatusBarMessage(string message);
        public abstract void AddProjectToSolution(string projectFullPath);
        public abstract void AddItemToActiveProject(string itemFullPath);
        public abstract void SaveSolution(string solutionFullPath);
        public abstract string GetActiveNamespace();

        public void CancelWizard()
        {
            throw new WizardCancelledException();
        }
    }
}
