// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
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

        public void SetNewItemSetupTitle() => Title = string.Format(StringRes.NewItemTitle_SF, GetLocalizedTemplateTypeName(ConfigTemplateType).ToLower());

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
            NewItemGenController.Instance.CleanupTempGeneration();
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
            ChangesSummary.ResetSelection();
            SetNewItemSetupTitle();
            CleanStatus();
        }

        protected override void OnNext()
        {
            HasOverlayBox = false;
            base.OnNext();
            NewItemSetup.EditionVisibility = Visibility.Collapsed;
            SetChangesSummaryTitle();
            NavigationService.Navigate(new ChangesSummaryView());
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

        protected override async Task OnTemplatesAvailableAsync()
        {
            SetProjectConfigInfo();
            NewItemSetup.Initialize(true);

            await Task.CompletedTask;
        }

        protected override async Task OnNewTemplatesAvailableAsync()
        {
            UpdateCanFinish(false);
            _canGoBack = false;
            BackCommand.OnCanExecuteChanged();
            ShowFinishButton = false;
            EnableGoForward();
            NavigationService.Navigate(new NewItemSetupView());
            NewItemSetup.Initialize(true);

            await Task.CompletedTask;
        }

        public override UserSelection CreateUserSelection()
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
