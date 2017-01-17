using System.Collections.Generic;
using System.IO;

namespace Microsoft.Templates.Wizard.Vs
{
    public interface IVisualStudioShell
    {
        void ShowStatusBarMessage(string message);
        void AddProjectToSolution(string projectFullPath);
        void SaveSolution(string solutionFullPath);
        string GetSolutionFullName();
        string GetSolutionDirectory();
        string GetSolutionName();
        string GetSolutionFileName();
        string GetSolutionVsCategory();
        void SetSolutionVsCategory(string categoryName);
        string GetActiveProjectName();
        string GetActiveProjectPath();
        string GetActiveProjectDefaultNamespace();
        string GetSelectedItemPath(bool relativeToProject);
        string GetSelectedItemDefaultNamespace();
        void AddItemToActiveProject(string itemFullPath);
        void CancelWizard();
        void Navigate(string url);
        void OpenItem(string fileName);
    }
}