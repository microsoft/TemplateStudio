using EnvDTE;
using Microsoft.Templates.Wizard;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Extension
{
    public class VsGenShell : GenShell
    {
        private Lazy<DTE> _dte = new Lazy<DTE>(() => ServiceProvider.GlobalProvider.GetService(typeof(DTE)) as DTE, true);
        private DTE Dte => _dte.Value;

        public VsGenShell()
        {

        }

        public VsGenShell(Dictionary<string, string> replacements) : base(replacements)
        {

        }

        public override void AddItemToActiveProject(string itemFullPath)
        {
            var proj = GetActiveProject();
            proj.ProjectItems.AddFromFile(itemFullPath);
        }

        public override void AddProjectToSolution(string projectFullPath)
        {
            Dte.Solution.AddFromFile(projectFullPath);
        }

        public override string GetActiveNamespace()
        {
            if (Dte.SelectedItems.Count > 0)
            {
                var selectedItem = Dte.SelectedItems.Item(1);

                if (selectedItem.Project != null)
                {
                    return selectedItem.Project.Properties.GetSafeValue("DefaultNamespace");
                }
                if (selectedItem.ProjectItem != null)
                {
                    return $"{selectedItem.ProjectItem.Properties.GetSafeValue("DefaultNamespace")}";
                }
            }
            return null;
        }

        public override void SaveSolution(string solutionFullPath)
        {
            Dte.Solution.SaveAs(solutionFullPath);
        }

        public override void ShowStatusBarMessage(string message)
        {
            Dte.StatusBar.Text = message;
        }

        protected override string GetSelectedItemPath()
        {
            if (Dte.SelectedItems.Count > 0)
            {
                var item = Dte.SelectedItems.Item(1);
                if (item.Project != null)
                {
                    return Path.GetDirectoryName(item.Project.FullName);
                }
                if (item.ProjectItem != null)
                {
                    string fullPath = $"{item.ProjectItem.Properties.GetSafeValue("FullPath")}";
                    return Path.GetDirectoryName(fullPath);
                }
            }

            return GetActiveProjectPath();
        }

        protected override string GetActiveProjectName()
        {
            var p = GetActiveProject();

            return p?.Name;
        }

        protected override string GetActiveProjectPath()
        {
            var p = GetActiveProject();
            if (p != null)
            {
                return Path.GetDirectoryName(p.FileName);
            }
            else
            {
                return null;
            }
        }

        private Project GetActiveProject()
        {
            Project p = null;
            if (_dte != null)
            {
                Array projects = (Array)Dte.ActiveSolutionProjects;
                if (projects?.Length >= 1)
                {
                    p = (Project)projects.GetValue(0);
                }
            }
            return p;
        }
    }
}
