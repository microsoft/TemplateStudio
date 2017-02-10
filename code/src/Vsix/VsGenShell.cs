using EnvDTE;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.Templates.Core.Extensions;
using Microsoft.Templates.Wizard;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VSLangProj;

namespace Microsoft.Templates.Extension
{
    public class VsGenShell : GenShell
    {
        private Lazy<DTE> _dte = new Lazy<DTE>(() => ServiceProvider.GlobalProvider.GetService(typeof(DTE)) as DTE, true);
        private DTE Dte => _dte.Value;

        private Lazy<IVsUIShell> _uiShell = new Lazy<IVsUIShell>(() => ServiceProvider.GlobalProvider.GetService(typeof(SVsUIShell)) as IVsUIShell, true);
        private IVsUIShell UIShell => _uiShell.Value;

        public VsGenShell()
        {
            
        }

        public VsGenShell(Dictionary<string, string> replacements) : base(replacements)
        {

        }

        public override void AddItemToActiveProject(string itemFullPath)
        {
            //TODO: Improve performance (allow passing various files to add)
            var proj = GetActiveProject();
            proj.ProjectItems.AddFromFile(itemFullPath);

            proj.Save();
        }

        public override bool SetActiveConfigurationAndPlatform(string configurationName, string platformName)
        {
            foreach (SolutionConfiguration solConfiguration in Dte.Solution.SolutionBuild.SolutionConfigurations)
            {
                if (solConfiguration.Name == configurationName)
                {
                    foreach (SolutionContext context in solConfiguration.SolutionContexts)
                    {
                        if (context.PlatformName == platformName)
                        {
                            solConfiguration.Activate();
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public override void AddProjectToSolution(string projectFullPath)
        {
            Dte.Solution.AddFromFile(projectFullPath);
            Dte.Solution.SaveAs(Dte.Solution.FileName);
        }
        
        public override void AddReferenceToProject(string projectName, string referenceProjectName)
        {
            var project = GetProjectByName(projectName);
            var referenceProject = GetProjectByName(referenceProjectName);

            if (project != null && referenceProject != null)
            {
                var vsProj = (VSLangProj.VSProject)project.Object;
                vsProj.References.AddProject(referenceProject);

                project.Save();
            }
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

            var p = GetActiveProject();
            if (p != null)
            {
                return p.Properties.GetSafeValue("DefaultNamespace");
            }

            return ProjectName;
        }

        public override void SaveSolution(string solutionFullPath)
        {
            Dte.Solution.SaveAs(solutionFullPath);
        }

        public override void ShowStatusBarMessage(string message)
        {
            Dte.StatusBar.Text = message;
        }

        public override void ShowTaskList()
        {
            ShowTaskListAsync().FireAndForget();
        }

        public override void ShowModal(System.Windows.Window dialog)
        {
            //get the owner of this dialog
            IntPtr hwnd;
            UIShell.GetDialogOwnerHwnd(out hwnd);
            dialog.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            UIShell.EnableModeless(0);
            try
            {
                WindowHelper.ShowModal(dialog, hwnd);
            }
            finally
            {
                // This will take place after the window is closed.
                UIShell.EnableModeless(1);
            }
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

        private Project GetProjectByName(string projectName)
        {
            foreach (EnvDTE.Project project in Dte.Solution.Projects)
            {
                if (project.Name == projectName)
                {
                    return project;
                }
            }
            return null;
        }

        private async System.Threading.Tasks.Task ShowTaskListAsync()
        {
            //JAVIERS: DELAY THIS EXECUTION TO OPEN THE WINDOW AFTER EVERYTHING IS LOADED
            await System.Threading.Tasks.Task.Delay(1000);

            var window = Dte.Windows.Item(EnvDTE.Constants.vsWindowKindTaskList);
            window.Activate();
        }
    }
}
