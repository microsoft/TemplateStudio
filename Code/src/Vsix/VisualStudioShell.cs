using System;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using System.IO;
using Microsoft.VisualStudio.Shell;
using Microsoft.Templates.Wizard.Vs;

namespace Microsoft.Templates.Extension
{
    public class VisualStudioShell : IVisualStudioShell
    {
        private DTE _dte;
        public VisualStudioShell()
        {
            _dte = ServiceProvider.GlobalProvider.GetService(typeof(DTE)) as DTE;
        }

        public void ShowStatusBarMessage(string message)
        {
            message.VsShowStatusBarMessage();
        }

        public void SaveSolution(string solutionFullPath)
        {
            _dte?.Solution?.SaveAs(solutionFullPath);
        }

        public string GetSolutionFullName()
        {
            if (_dte?.Solution != null)
            {
                return _dte.Solution.FullName;
            }
            else
            {
                return String.Empty;
            }
        }

        public string GetSolutionDirectory()
        {
            if (!String.IsNullOrEmpty(_dte?.Solution?.FullName))
            {
                return Path.GetDirectoryName(_dte.Solution.FullName);
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetSolutionName()
        {
            if (!String.IsNullOrEmpty(_dte?.Solution?.FullName))
            {
                return Path.GetFileNameWithoutExtension(_dte.Solution.FullName);
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetSolutionFileName()
        {
            if (!String.IsNullOrEmpty(_dte?.Solution?.FullName))
            {
                return Path.GetFileName(_dte.Solution.FullName);
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetSolutionVsCategory()
        {
            if (_dte.Solution.Globals.VariableExists["UwpTemplatesCategory"])
            {
                return (string)_dte.Solution.Globals["UwpTemplatesCategory"];
            }
            else
            {
                return String.Empty;
            }
        }

        public void SetSolutionVsCategory(string categoryName)
        {
            Globals globals = _dte.Solution.Globals;

            if (!globals.get_VariableExists("UwpTemplatesCategory"))
            {
                globals["UwpTemplatesCategory"] = categoryName.ToString();
                globals.set_VariablePersists("UwpTemplatesCategory", true);
            }
        }

        public void AddProjectToSolution(string projectFullPath)
        {
            if (_dte != null)
            {
                ShowStatusBarMessage($"Adding '{projectFullPath}'...");
                _dte.Solution.AddFromFile(projectFullPath);
            }
        }

        public void CancelWizard()
        {
            throw new WizardCancelledException();
        }

        public string GetActiveProjectName()
        {
            Project p = GetActiveProject();
            if (p != null)
            {
                return p.Name;
            }
            else
            {
                return String.Empty;
            }
        }

        public string GetActiveProjectPath()
        {
            Project p = GetActiveProject();
            if (p != null)
            {
                return Path.GetDirectoryName(p.FileName);
            }
            else
            {
                return String.Empty;
            }
        }
        public void AddItemToActiveProject(string itemFullPath)
        {
            Project proj = GetActiveProject();
            proj?.ProjectItems.AddFromFile(itemFullPath);
        }

        public void Navigate(string url)
        {
            if(_dte != null && !String.IsNullOrEmpty(url)){
                _dte.ItemOperations.Navigate(url, vsNavigateOptions.vsNavigateOptionsNewWindow);
            }
        }
        public void OpenItem(string fileName)
        {
            if (_dte != null && !String.IsNullOrEmpty(fileName))
            {
                _dte.ItemOperations.OpenFile(fileName);
            }
        }
        public string GetActiveProjectDefaultNamespace()
        {
            Project p = GetActiveProject();
            if (p != null)
            {
                return p?.Properties.GetSafeValue("DefaultNamespace");
            }
            else
            {
                return String.Empty;
            }
        }

        public string GetSelectedItemPath(bool relativeToProject)
        {
            string result = String.Empty;
            if (_dte.SelectedItems.Count > 0)
            {
                SelectedItem item = _dte.SelectedItems.Item(1);
                if (item.Project != null)
                {
                    result = Path.GetDirectoryName(item.Project.FullName);
                }
                if (item.ProjectItem != null)
                {
                    string fullPath = $"{item.ProjectItem.Properties.GetSafeValue("FullPath")}";
                    string directory = Path.GetDirectoryName(fullPath);
                    result = directory;
                }
            }
            if(!String.IsNullOrEmpty(result) && relativeToProject)
            {
                result = result.Replace(GetActiveProjectPath(), "").RemoveStartDirectorySparator().RemoveTailDirectorySparator();

            }
            return result;
        }

        public string GetSelectedItemDefaultNamespace()
        {
            string result = String.Empty;
            if (_dte?.SelectedItems.Count > 0)
            {
                SelectedItem item = _dte.SelectedItems.Item(1);
                if (item.Project != null)
                {
                    result = item.Project.Properties.GetSafeValue("DefaultNamespace");
                }
                if (item.ProjectItem != null)
                {
                    result = $"{item.ProjectItem.Properties.GetSafeValue("DefaultNamespace")}";
                }
            }
            return result;
        }


        private Project GetActiveProject()
        {
            Project p = null;
            if (_dte != null)
            {
                Array projects = (Array)_dte.ActiveSolutionProjects;
                if (projects?.Length >= 1)
                {
                    p = (Project)projects.GetValue(0);
                }
            }
            return p;
        }
    }
}