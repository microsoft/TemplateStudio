using EnvDTE;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.Templates.Core.Extensions;
using Microsoft.Templates.Core.Gen;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TemplateWizard;
using NuGet.VisualStudio;
using System;
using System.IO;

namespace Microsoft.Templates.Wizard.VisualStudio
{
    public class VsGenShell : GenShell
    {
        private Lazy<DTE> _dte = new Lazy<DTE>(() => ServiceProvider.GlobalProvider.GetService(typeof(DTE)) as DTE, true);
        private DTE Dte => _dte.Value;

        private Lazy<IVsUIShell> _uiShell = new Lazy<IVsUIShell>(() => ServiceProvider.GlobalProvider.GetService(typeof(SVsUIShell)) as IVsUIShell, true);
        private IVsUIShell UIShell => _uiShell.Value;

        public IVsPackageRestorer packageRestorer;

        //TODO: CACHE ON THIS
        private VsOutputPane OutputPane => new VsOutputPane();
        
        public override void AddItems(params string[] itemsFullPath)
        {
            if (itemsFullPath == null || itemsFullPath.Length == 0)
            {
                return;
            }
            var proj = GetActiveProject();
            foreach (var item in itemsFullPath)
            {
                proj.ProjectItems.AddFromFile(item);
            }

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

            try
            {
                if (_dte != null)
                {
                    Array projects = (Array)Dte.ActiveSolutionProjects;
                    if (projects?.Length >= 1)
                    {
                        p = (Project)projects.GetValue(0);
                    }
                }
            }
            catch (Exception)
            {
                //WE GE AN EXCEPTION WHEN THERE ISN'T A PROJECT LOADED
            }
            return p;
        }

        private async System.Threading.Tasks.Task ShowTaskListAsync()
        {
            //JAVIERS: DELAY THIS EXECUTION TO OPEN THE WINDOW AFTER EVERYTHING IS LOADED
            await System.Threading.Tasks.Task.Delay(1000);

            var window = Dte.Windows.Item(EnvDTE.Constants.vsWindowKindTaskList);
            window.Activate();
        }

        public override void WriteOutput(string data)
        {
            OutputPane.Write(data);
        }

        public override void CloseSolution()
        {
            Dte.Solution.Close();
        }

        public override void RestorePackages()
        {
            var componentModel = (IComponentModel)Package.GetGlobalService(typeof(SComponentModel));
            var restoreService = componentModel.GetService<IVsPackageRestorer>();

            restoreService?.RestorePackages(GetActiveProject());
        }
    }
}
