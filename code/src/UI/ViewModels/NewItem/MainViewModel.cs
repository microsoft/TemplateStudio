// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.UI.Controls;
using Microsoft.Templates.UI.Generation;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.ViewModels.Common;
using Microsoft.Templates.UI.Views.NewItem;

namespace Microsoft.Templates.UI.ViewModels.NewItem
{
    public class MainViewModel : BaseMainViewModel
    {
        public static MainViewModel Current;
        public MainView MainView;

        // Configuration
        public TemplateType ConfigTemplateType;
        public string ConfigFramework;
        public string ConfigProjectType;
        public NewItemSetupViewModel NewItemSetup { get; private set; } = new NewItemSetupViewModel();
        public ChangesSummaryViewModel ChangesSummary { get; private set; } = new ChangesSummaryViewModel();

        public MainViewModel(MainView mainView) : base(mainView)
        {
            MainView = mainView;
            Current = this;
        }

        public async Task InitializeAsync(TemplateType templateType)
        {
            ConfigTemplateType = templateType;
            SetNewItemSetupTitle();
            await BaseInitializeAsync();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008:Opening parenthesis must be spaced correctly", Justification = "Using tuples must allow to have preceding whitespace", Scope = "member")]
        private void SetProjectConfigInfo()
        {
            var configInfo = ProjectConfigInfo.ReadProjectConfiguration();
            if (string.IsNullOrEmpty(configInfo.ProjectType) || string.IsNullOrEmpty(configInfo.Framework))
            {
                InfoShapeVisibility = System.Windows.Visibility.Visible;
                ProjectConfigurationWindow projectConfig = new ProjectConfigurationWindow(MainView);
                if (projectConfig.ShowDialog().Value)
                {
                    configInfo.ProjectType = projectConfig.ViewModel.SelectedProjectType.Name;
                    configInfo.Framework = projectConfig.ViewModel.SelectedFramework.Name;
                    InfoShapeVisibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    Cancel();
                }
            }
            ConfigFramework = configInfo.Framework;
            ConfigProjectType = configInfo.ProjectType;
        }

        public void SetNewItemSetupTitle() => Title = string.Format(StringRes.NewItemTitle_SF, this.GetLocalizedTemplateTypeName(ConfigTemplateType).ToLower());

        private string GetLocalizedTemplateTypeName(TemplateType templateType)
        {
            switch (templateType)
            {
                case TemplateType.Feature:
                    return StringRes.TemplateTypeFeature;
                case TemplateType.Page:
                    return StringRes.TemplateTypePage;
                case TemplateType.Project:
                    return StringRes.TemplateTypeProjectType;
                default:
                    return templateType.ToString();
            }
        }

        public void SetChangesSummaryTitle()
        {
            var template = GetActiveTemplate();
            if (template.IsItemNameEditable)
            {
                Title = string.Format(StringRes.ChangesSummaryTitle_SF, NewItemSetup.ItemName, template.TemplateType.ToString().ToLower());
            }
            else
            {
                Title = string.Format(StringRes.ChangesSummaryTitle_SF, template.Name, template.TemplateType.ToString().ToLower());
            }
        }

        protected override void OnCancel()
        {
            Cancel();
        }

        private void Cancel()
        {
            MainView.DialogResult = false;
            MainView.Result = null;
            MainView.Close();
        }

        protected override void OnClose()
        {
            MainView.DialogResult = true;
            MainView.Result = null;
            MainView.Close();
        }

        protected override void OnGoBack()
        {
            base.OnGoBack();
            NewItemSetup.Initialize(false);
            HasOverlayBox = true;
            ChangesSummary.HasLicenses = false;
            ChangesSummary.Licenses.Clear();
            SetNewItemSetupTitle();
            CleanStatus();
        }
        protected override async void OnNext()
        {
            HasOverlayBox = false;
            base.OnNext();
            NewItemSetup.EditionVisibility = Visibility.Collapsed;
            SetStatus(new StatusViewModel(StatusType.Information, StringRes.GenerationFeedbackMessage));
            MainView.Result = CreateUserSelection();
            NewItemGenController.Instance.CleanupTempGeneration();
            await NewItemGenController.Instance.GenerateNewItemAsync(ConfigTemplateType, MainView.Result);
            NavigationService.Navigate(new ChangesSummaryView());
            SetChangesSummaryTitle();
        }
        protected override void OnFinish(string parameter)
        {
            MainView.Result.ItemGenerationType = ChangesSummary.DoNotMerge ? ItemGenerationType.Generate : ItemGenerationType.GenerateAndMerge;
            base.OnFinish(parameter);
        }

        public TemplateInfoViewModel GetActiveTemplate()
        {
            var activeGroup = NewItemSetup.TemplateGroups.FirstOrDefault(gr => gr.SelectedItem != null);
            if (activeGroup != null)
            {
                return activeGroup.SelectedItem as TemplateInfoViewModel;
            }
            return null;
        }

        protected override void OnTemplatesAvailable()
        {
            SetProjectConfigInfo();
            NewItemSetup.Initialize(true);
        }

        protected override void OnNewTemplatesAvailable()
        {
            UpdateCanFinish(false);
            _canGoBack = false;
            BackCommand.OnCanExecuteChanged();
            ShowFinishButton = false;
            EnableGoForward();
            NavigationService.Navigate(new NewItemSetupView());
            NewItemSetup.Initialize(true);
        }
        protected override UserSelection CreateUserSelection()
        {
            var userSelection = new UserSelection()
            {
                Framework = ConfigFramework,
                ProjectType = ConfigProjectType,
                HomeName = string.Empty
            };
            var template = GetActiveTemplate();
            if (template != null)
            {
                var dependencies = GenComposer.GetAllDependencies(template.Template, ConfigFramework);

                userSelection.Pages.Clear();
                userSelection.Features.Clear();

                AddTemplate(userSelection, NewItemSetup.ItemName, template.Template, ConfigTemplateType);

                foreach (var dependencyTemplate in dependencies)
                {
                    AddTemplate(userSelection, dependencyTemplate.GetDefaultName(), dependencyTemplate, dependencyTemplate.GetTemplateType());
                }
            }
            return userSelection;
        }

        private void AddTemplate(UserSelection userSelection, string name, ITemplateInfo template, TemplateType templateType)
        {
            if (templateType == TemplateType.Page)
            {
                userSelection.Pages.Add((name, template));
            }
            else if (templateType == TemplateType.Feature)
            {
                userSelection.Features.Add((name, template));
            }
        }
    }
}
