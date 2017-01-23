using Microsoft.Templates.Wizard.Dialog;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Templates.Wizard.Vs;
using Microsoft.Templates.Core;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Wizard.Resources;

namespace Microsoft.Templates.Wizard
{
    public class GenerationWizard
    {
        TemplatesRepository _templatesRepository;
        IVisualStudioShell _vsShell;
        SolutionInfo _vsSolutionInfo;

        AddProjectResult _addProjectResult;

        public GenerationWizard(IVisualStudioShell vsShell, SolutionInfo solutionInfo) 
            : this(vsShell, solutionInfo, new TemplatesRepository(new CdnTemplatesLocation()))
        {
        }

        public GenerationWizard(IVisualStudioShell currentVsShell, SolutionInfo solutionInfo, TemplatesRepository repository)
        {
            _vsShell = currentVsShell;
            _vsSolutionInfo = solutionInfo;
            _templatesRepository = repository;
            _templatesRepository.Sync();
        }

        public void AddProjectInit()
        {
            MessageBox.Show($"{Configuration.Current.CdnUrl}\r\n{Configuration.Current.AppInsightsKey}" );

            _vsShell.ShowStatusBarMessage(StringRes.UIAddProjectAdding);
            _addProjectResult = ShowAddProjectDialog(_vsSolutionInfo.Name, _vsSolutionInfo.TemplateCategory);

            if (_addProjectResult == null)
            {
                _vsShell.ShowStatusBarMessage(StringRes.UIActionCancelled);
                _vsShell.CancelWizard();
            }
        }
        public void AddProjectFinish()
        {
            try
            {
                if (_addProjectResult != null)
                {

                    _vsShell.ShowStatusBarMessage(StringRes.UIAddProjectGenerating);

                    GenerateProject(_addProjectResult.ProjectTemplate, _vsSolutionInfo, _vsShell);

                    _vsShell.SetSolutionVsCategory(_vsSolutionInfo.TemplateCategory);

                    ShowReadMe(_addProjectResult.ProjectTemplate);

                    _vsShell.ShowStatusBarMessage(StringRes.UIProjectSuccessfullyCreated);
                }
            }
            catch (Exception ex)
            {
                ShowException(StringRes.UIErrorProjectCantBeGenerated, ex);
            }
        }


        public void AddPageToActiveProject()
        {
            MessageBox.Show($"{Configuration.Current.CdnUrl}\r\n{Configuration.Current.AppInsightsKey}");
            if (_vsShell.GetActiveProjectName() != "")
            {
                _vsShell.ShowStatusBarMessage(StringRes.UIAddPage);

                string suggestedNamespace = _vsShell.GetSelectedItemDefaultNamespace();

                AddPageResult result = ShowAddPageDialog(_vsShell.GetActiveProjectName(), _vsSolutionInfo.TemplateCategory, suggestedNamespace);
                if (result != null)
                {
                    string relativeItemPath = _vsShell.GetSelectedItemPath(true);

                    GeneratePage(result.PageTemplate, _vsShell.GetActiveProjectName(), _vsShell.GetActiveProjectPath(), result.Namespace, result.PageName, relativeItemPath, _vsShell);
                    _vsShell.ShowStatusBarMessage(StringRes.UIPageAddedPattern.UseParams(result.PageName));
                }
                else
                {
                    _vsShell.ShowStatusBarMessage(StringRes.UIActionCancelled);
                }
            }
            else
            {
                MessageBox.Show(StringRes.UINoActiveProjectMessage, StringRes.UIMessageBoxTitlePattern.UseParams(StringRes.AddPageAction), MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        public void AddFeatureToActiveProject()
        {
            AddFeaturesResult result = ShowAddFeaturesDialog(_vsShell.GetActiveProjectName(), _vsSolutionInfo.TemplateCategory);
            if (result != null)
            {
                //TODO: Generate Feature Code

                //TODO: Include generated code
            }
            else
            {
                _vsShell.ShowStatusBarMessage(StringRes.UIActionCancelled);
            }
        }

        private AddProjectResult ShowAddProjectDialog(string targetProjectName, string vsTemplateCategory)
        {
            try {
                AddNewProject addProjectDialog = new AddNewProject(targetProjectName, vsTemplateCategory, _templatesRepository, AddProjectSteps.SelectProject);
                var result = addProjectDialog.ShowDialog();

                return (result.HasValue && result.Value ? addProjectDialog.ResultInfo : null);
            }
            catch (Exception ex)
            {
                ShowException(StringRes.UIErrorProjectCantBeGenerated, ex);
                return null;
            }
        }

        private AddFeaturesResult ShowAddFeaturesDialog(string targetProjectName, string vsTemplateCategory)
        {
            try
            {
                MessageBox.Show("Not implemented!", StringRes.UIMessageBoxTitlePattern.UseParams(StringRes.AddFeatureAction), MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return null;
            }
            catch (Exception ex)
            {
                ShowException(StringRes.UIErrorFeatureCantBeAdded, ex);
                return null;
            }
        }

        private AddPageResult ShowAddPageDialog(string targetProjectName, string vsTemplateCategory, string suggestedNamespace)
        {
            try
            {
                AddPage addPageDialog = new AddPage(targetProjectName, vsTemplateCategory, suggestedNamespace, _templatesRepository);
                var result = addPageDialog.ShowDialog();
                return (result.HasValue && result.Value ? addPageDialog.ResultInfo : null);
            }
            catch (Exception ex)
            {
                ShowException(StringRes.UIErrorPageCantBeAdded, ex);
                return null;
            }
        }

        private static void GenerateProject(ITemplateInfo template, SolutionInfo solutionInfo, IVisualStudioShell vsShell)
        {
            var result = TemplateCreator.InstantiateAsync(template, solutionInfo.Name, null, solutionInfo.Directory, new Dictionary<string, string>(), true).Result;

            if (result.Status == CreationResultStatus.CreateSucceeded && result.PrimaryOutputs != null)
            {
                foreach (var output in result.PrimaryOutputs)
                {
                    if (!string.IsNullOrWhiteSpace(output.Path))
                    {
                        var projectPath = Path.GetFullPath(Path.Combine(solutionInfo.Directory, output.Path));
                        vsShell.AddProjectToSolution(projectPath); 
                    }
                } 
            }
        }

        private static void GeneratePage(ITemplateInfo template, string projectName, string projectPath, string pageNamespace, string pageName, string pageRelativePath, IVisualStudioShell vsShell)
        {
            var genParams = new Dictionary<string, string>();
            genParams.Add("PageNamespace", pageNamespace);

            string generationPath = Path.Combine(projectPath, pageRelativePath);
            var result = TemplateCreator.InstantiateAsync(template, pageName, null, generationPath, genParams, true).Result;
            
            //TODO: Control overwrites! What happend if the generated content already exits.

            if (result.Status == CreationResultStatus.CreateSucceeded && result.PrimaryOutputs != null)
            {
                foreach (var output in result.PrimaryOutputs)
                {
                    if (!string.IsNullOrWhiteSpace(output.Path))
                    {
                        var itemPath = Path.GetFullPath(Path.Combine(generationPath, output.Path));
                        vsShell.AddItemToActiveProject(itemPath);
                    }
                }
            }

            //TODO:Post Action --> Show ViewModelLocator Information
            //TODO: Show Project Information
        }

        private void ShowReadMe(ITemplateInfo template)
        {
            //TODO: GoTo formula Readme / Help page
            _vsShell.Navigate("https://github.com/Microsoft/UWPCommunityTemplates/tree/vnext");
        }

        private void ShowException(string message, Exception ex)
        {
            //TODO: Create a class to show exceptions (and log them to a file). Show detailed info depending on setting value;
            ErrorMessageDialog.Show(message, StringRes.UIMessageBoxTitlePattern.UseParams(StringRes.UnexpectedError), ex.ToString(), MessageBoxImage.Error);
        }
    }
}
