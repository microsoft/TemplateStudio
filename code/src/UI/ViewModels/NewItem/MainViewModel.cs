// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.UI.Generation;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.Threading;
using Microsoft.Templates.UI.ViewModels.Common;
using Microsoft.Templates.UI.Views.NewItem;

namespace Microsoft.Templates.UI.ViewModels.NewItem
{
    public enum NewItemStep
    {
        ItemConfiguration = 0,
        ChangesSummary = 1
    }

    public class MainViewModel : BaseMainViewModel
    {
        private readonly string _language;

        public static MainViewModel Current { get; private set; }

        public MainView MainView { get; private set; }

        // Configuration
        public TemplateType ConfigTemplateType { get; private set; }

        public string ConfigFramework { get; private set; }

        public string ConfigProjectType { get; private set; }

        public NewItemSetupViewModel NewItemSetup { get; private set; } = new NewItemSetupViewModel();

        public ChangesSummaryViewModel ChangesSummary { get; private set; } = new ChangesSummaryViewModel();

        public MainViewModel(string language)
            : base()
        {
            _language = language;
            Current = this;
        }

        public override void SetView(Window window)
        {
            base.SetView(window);
            MainView = window as MainView;
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
                WizardStatus.InfoShapeVisibility = Visibility.Visible;
                ProjectConfigurationWindow projectConfig = new ProjectConfigurationWindow(MainView);

                if (projectConfig.ShowDialog().Value)
                {
                    configInfo.ProjectType = projectConfig.ViewModel.SelectedProjectType.Name;
                    configInfo.Framework = projectConfig.ViewModel.SelectedFramework.Name;
                    WizardStatus.InfoShapeVisibility = Visibility.Collapsed;
                }
                else
                {
                    Cancel();
                }
            }

            ConfigFramework = configInfo.Framework;
            ConfigProjectType = configInfo.ProjectType;
        }

        public void SetNewItemSetupTitle()
        {
            if (ConfigTemplateType == TemplateType.Page)
            {
                WizardStatus.WizardTitle = StringRes.NewItemTitlePage;
            }
            else if (ConfigTemplateType == TemplateType.Feature)
            {
                WizardStatus.WizardTitle = StringRes.NewItemTitleFeature;
            }
        }

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

            switch (template.TemplateType)
            {
                case TemplateType.Page:
                    WizardStatus.WizardTitle = string.Format(StringRes.ChangesSummaryTitlePage, template.IsItemNameEditable ? NewItemSetup.ItemName : template.Name);
                    break;
                case TemplateType.Feature:
                    WizardStatus.WizardTitle = string.Format(StringRes.ChangesSummaryTitleFeature, template.IsItemNameEditable ? NewItemSetup.ItemName : template.Name);
                    break;
            }
        }

        protected override void OnCancel()
        {
            Cancel();
        }

        private void Cancel()
        {
            MainView.DialogResult = false;
        }

        protected override void OnClose()
        {
            MainView.DialogResult = true;
        }

        protected override void OnGoBack()
        {
            base.OnGoBack();
            NewItemSetup.Initialize(false);
            WizardStatus.HasOverlayBox = true;
            ChangesSummary.ResetSelection();
            SetNewItemSetupTitle();
            CleanStatus();
        }

        protected override void OnNext()
        {
            if (CurrentStep == 0)
            {
                UpdateCanGoForward(false);

                SafeThreading.JoinableTaskFactory.Run(async () =>
                {
                    await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                    var output = await CleanupAndGenerateNewItemAsync();
                    if (output.HasChangesToApply)
                    {
                        base.OnNext();
                        await EnsureCodeViewerInitializedAsync();
                        WizardStatus.HasOverlayBox = false;
                        NewItemSetup.EditionVisibility = Visibility.Collapsed;
                        SetChangesSummaryTitle();
                        NavigationService.Navigate(new ChangesSummaryView(output));
                    }
                    else
                    {
                        UpdateCanGoForward(true);
                        switch (ConfigTemplateType)
                        {
                            case TemplateType.Page:
                                WizardStatus.SetStatus(StatusViewModel.Warning(string.Format(StringRes.NewItemHasNoChangesPage, NewItemSetup.ItemName), true, 5));
                                break;
                            case TemplateType.Feature:
                                WizardStatus.SetStatus(StatusViewModel.Warning(string.Format(StringRes.NewItemHasNoChangesFeature, NewItemSetup.ItemName), true, 5));
                                break;
                        }
                    }
                });
            }
        }

        private async Task EnsureCodeViewerInitializedAsync()
        {
            WizardStatus.IsLoading = true;
            await Task.Delay(300);
        }

        private async Task<NewItemGenerationResult> CleanupAndGenerateNewItemAsync()
        {
            MainView.Result = CreateUserSelection();
            NewItemGenController.Instance.CleanupTempGeneration();
            await NewItemGenController.Instance.GenerateNewItemAsync(ConfigTemplateType, MainView.Result);

            var output = NewItemGenController.Instance.CompareOutputAndProject();
            return output;
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
            NavigationService.Navigate(new NewItemSetupView());
            NewItemSetup.Initialize(true);

            await Task.CompletedTask;
        }

        public UserSelection CreateUserSelection()
        {
            var userSelection = new UserSelection(ConfigProjectType, ConfigFramework, _language)
            {
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
            userSelection.Add((name, template));
        }
    }
}
