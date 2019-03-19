// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Services;
using Microsoft.Templates.UI.Controls;
using Microsoft.Templates.UI.Mvvm;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.Threading;
using Microsoft.Templates.UI.ViewModels.Common;
using Microsoft.Templates.UI.Views.Common;
using Microsoft.Templates.UI.Views.NewItem;

namespace Microsoft.Templates.UI.ViewModels.NewItem
{
    public class MainViewModel : BaseMainViewModel
    {
        private RelayCommand _refreshTemplatesCacheCommand;

        private NewItemGenerationResult _output;

        private GenerationService _generationService = GenerationService.Instance;

        private string _emptyBackendFramework = string.Empty;

        public TemplateType TemplateType { get; set; }

        public string ConfigPlatform { get; private set; }

        public string ConfigFramework { get; private set; }

        public string ConfigProjectType { get; private set; }

        public static MainViewModel Instance { get; private set; }

        public TemplateSelectionViewModel TemplateSelection { get; } = new TemplateSelectionViewModel();

        public ObservableCollection<BreakingChangeMessageViewModel> BreakingChangesErrors { get; set; } = new ObservableCollection<BreakingChangeMessageViewModel>();

        public ChangesSummaryViewModel ChangesSummary { get; } = new ChangesSummaryViewModel();

        public RelayCommand RefreshTemplatesCacheCommand => _refreshTemplatesCacheCommand ?? (_refreshTemplatesCacheCommand = new RelayCommand(
            () => SafeThreading.JoinableTaskFactory.RunAsync(async () => await OnRefreshTemplatesAsync())));

        public Visibility RefreshTemplateCacheVisibility
        {
            get
            {
#if DEBUG
                return Visibility.Visible;
#else
                return Visibility.Collapsed;
#endif
            }
        }

        public MainViewModel(WizardShell mainWindow, BaseStyleValuesProvider provider)
            : base(mainWindow, provider, false)
        {
            Instance = this;
        }

        public async Task InitializeAsync(TemplateType templateType, string language)
        {
            TemplateType = templateType;
            var stringResource = templateType == TemplateType.Page ? StringRes.NewItemTitlePage : StringRes.NewItemTitleFeature;
            WizardStatus.Title = stringResource;
            SetProjectConfigInfo();
            await InitializeAsync(ConfigPlatform, language);
        }

        protected override async Task<bool> IsStepAvailableAsync(int step)
        {
            if (step == 1 && !WizardStatus.HasValidationErrors)
            {
                _output = await CleanupAndGenerateNewItemAsync();
                if (!_output.HasChangesToApply)
                {
                    var message = TemplateType == TemplateType.Page ? StringRes.NewItemHasNoChangesPage : StringRes.NewItemHasNoChangesFeature;
                    message = string.Format(message, TemplateSelection.Name);
                    var notification = Notification.Warning(message, Category.RightClickItemHasNoChanges);
                    NotificationsControl.AddNotificationAsync(notification).FireAndForget();
                }

                return _output.HasChangesToApply;
            }

            return await base.IsStepAvailableAsync(step);
        }

        protected override void UpdateStep(bool navigate)
        {
            base.UpdateStep(navigate);
            if (Step == 0)
            {
                ChangesSummary.ClearSelected();
            }

            SetCanFinish(Step > 0);
        }

        private async Task<NewItemGenerationResult> CleanupAndGenerateNewItemAsync()
        {
            NewItemGenController.Instance.CleanupTempGeneration();
            var userSelection = CreateUserSelection();
            await _generationService.GenerateNewItemAsync(TemplateSelection.Template.TemplateType, userSelection);
            return NewItemGenController.Instance.CompareOutputAndProject();
        }

        private UserSelection CreateUserSelection()
        {
            var userSelection = new UserSelection(ConfigProjectType, ConfigFramework, _emptyBackendFramework, ConfigPlatform, Language) { HomeName = string.Empty };
            var selectedTemplate = new UserSelectionItem { Name = TemplateSelection.Name, TemplateId = TemplateSelection.Template.TemplateId };
            userSelection.Add(selectedTemplate, TemplateSelection.Template.TemplateType);

            foreach (var dependencyTemplate in TemplateSelection.Template.Dependencies)
            {
                var selectedTemplateDependency = new UserSelectionItem { Name = dependencyTemplate.DefaultName, TemplateId = dependencyTemplate.TemplateId };
                userSelection.Add(selectedTemplateDependency, dependencyTemplate.TemplateType);
            }

            return userSelection;
        }

        protected override void OnCancel() => WizardShell.Current.Close();

        protected override void OnFinish()
        {
            WizardShell.Current.Result = GetUserSelection();
            WizardShell.Current.Result.ItemGenerationType = ChangesSummary.DoNotMerge ? ItemGenerationType.Generate : ItemGenerationType.GenerateAndMerge;
            base.OnFinish();
        }

        private UserSelection GetUserSelection()
        {
            var userSelection = new UserSelection(ConfigProjectType, ConfigFramework, _emptyBackendFramework, ConfigPlatform, Language);
            var selectedItem = new UserSelectionItem { Name = TemplateSelection.Name, TemplateId = TemplateSelection.Template.TemplateId };
            userSelection.Add(selectedItem, TemplateSelection.Template.TemplateType);
            return userSelection;
        }

        protected override async Task OnTemplatesAvailableAsync()
        {
            TemplateSelection.LoadData(TemplateType, ConfigPlatform, ConfigProjectType, ConfigFramework);
            WizardStatus.IsLoading = false;

            var result = BreakingChangesValidatorService.Validate();
            if (!result.IsValid)
            {
                var messages = result.ErrorMessages.Select(e => new BreakingChangeMessageViewModel(e));
                BreakingChangesErrors.AddRange(messages);
                OnPropertyChanged(nameof(BreakingChangesErrors));

                await Task.CompletedTask;
            }
        }

        protected async Task OnRefreshTemplatesAsync()
        {
            try
            {
                WizardStatus.IsLoading = true;
                await GenContext.ToolBox.Repo.RefreshAsync(true);
            }
            catch (Exception ex)
            {
                await NotificationsControl.AddNotificationAsync(Notification.Error(StringRes.NotificationSyncError_Refresh));

                await AppHealth.Current.Error.TrackAsync(ex.ToString());
                await AppHealth.Current.Exception.TrackAsync(ex);
            }
            finally
            {
                WizardStatus.IsLoading = GenContext.ToolBox.Repo.SyncInProgress;
            }
        }

        private void SetProjectConfigInfo()
        {
            var configInfo = ProjectConfigInfo.ReadProjectConfiguration();
            if (string.IsNullOrEmpty(configInfo.ProjectType) || string.IsNullOrEmpty(configInfo.Framework) || string.IsNullOrEmpty(configInfo.Platform))
            {
                var vm = new ProjectConfigurationViewModel();
                ProjectConfigurationDialog projectConfig = new ProjectConfigurationDialog(vm);
                projectConfig.Owner = WizardShell.Current;
                projectConfig.ShowDialog();

                if (vm.Result == DialogResult.Accept)
                {
                    configInfo.ProjectType = vm.SelectedProjectType.Name;
                    configInfo.Framework = vm.SelectedFramework.Name;
                    configInfo.Platform = vm.SelectedPlatform;
                    ProjectMetadataService.SaveProjectMetadata(configInfo);
                    ConfigFramework = configInfo.Framework;
                    ConfigProjectType = configInfo.ProjectType;
                    ConfigPlatform = configInfo.Platform;
                }
                else
                {
                    OnCancel();
                }
            }
            else
            {
                ConfigFramework = configInfo.Framework;
                ConfigProjectType = configInfo.ProjectType;
                ConfigPlatform = configInfo.Platform;
            }
        }

        protected override IEnumerable<Step> GetSteps()
        {
            yield return new Step(0, StringRes.NewItemStepOne, () => new TemplateSelectionPage(), true, true);
            yield return new Step(1, StringRes.NewItemStepTwo, () => new ChangesSummaryPage(_output));
        }

        public override async Task ProcessItemAsync(object item)
        {
            if (item is TemplateInfoViewModel template)
            {
                TemplateSelection.SelectTemplate(template);
            }

            await Task.CompletedTask;
        }

        public override bool IsSelectionEnabled(MetadataType metadataType) => true;
    }
}
