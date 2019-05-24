// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.Controls;
using Microsoft.Templates.UI.Mvvm;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.Threading;
using Microsoft.Templates.UI.ViewModels.Common;
using Microsoft.Templates.UI.Views.Common;
using Microsoft.Templates.UI.Views.NewProject;

namespace Microsoft.Templates.UI.ViewModels.NewProject
{
    public class MainViewModel : BaseMainViewModel
    {
        public const string NewProjectStepProjectType = "ProjectType";
        public const string NewProjectStepFramework = "Framework";
        public const string NewProjectStepPages = "Pages";
        public const string NewProjectStepFeatures = "Features";
        public const string NewProjectStepTests = "Tests";
        public const string NewProjectStepServices = "Services";

        private RelayCommand _refreshTemplatesCacheCommand;
        private RelayCommand _compositionToolCommand;

        private TemplateInfoViewModel _selectedTemplate;

        public Dictionary<TemplateType, TemplatesStepViewModel> StepsViewModels { get; } = new Dictionary<TemplateType, TemplatesStepViewModel>();

        public static MainViewModel Instance { get; private set; }

        public ProjectTypeViewModel ProjectType { get; } = new ProjectTypeViewModel(() => Instance.IsSelectionEnabled(MetadataType.ProjectType), () => Instance.OnProjectTypeSelectedAsync());

        public FrameworkViewModel Framework { get; } = new FrameworkViewModel(() => Instance.IsSelectionEnabled(MetadataType.Framework), () => Instance.OnFrameworkSelectedAsync());

        public UserSelectionViewModel UserSelection { get; } = new UserSelectionViewModel();

        public CompositionToolViewModel CompositionTool { get; } = new CompositionToolViewModel();

        public RelayCommand RefreshTemplatesCacheCommand => _refreshTemplatesCacheCommand ?? (_refreshTemplatesCacheCommand = new RelayCommand(
             () => SafeThreading.JoinableTaskFactory.RunAsync(async () => await OnRefreshTemplatesCacheAsync())));

        public RelayCommand CompositionToolCommand => _compositionToolCommand ?? (_compositionToolCommand = new RelayCommand(() => OnCompositionTool()));

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

        public MainViewModel(Window mainView, BaseStyleValuesProvider provider)
            : base(mainView, provider, NewProjectSteps)
        {
            Instance = this;
            ValidationService.Initialize(UserSelection.GetNames, UserSelection.GetPageNames);
            Navigation.OnFinish += OnFinish;
        }

        public override void UnsubscribeEventHandlers()
        {
            base.UnsubscribeEventHandlers();
            Navigation.OnFinish -= OnFinish;
        }

        private static IEnumerable<StepData> NewProjectSteps
        {
            get
            {
                yield return StepData.MainStep(NewProjectStepProjectType, "1", StringRes.NewProjectStepOne, () => new ProjectTypePage(), true, true);
                yield return StepData.MainStep(NewProjectStepFramework, "2", StringRes.NewProjectStepTwo, () => new FrameworkPage());
                yield return StepData.MainStep(NewProjectStepPages, "3", StringRes.NewProjectStepThree, () => new TemplatesStepPage(TemplateType.Page));
                yield return StepData.MainStep(NewProjectStepFeatures, "4", StringRes.NewProjectStepFour, () => new TemplatesStepPage(TemplateType.Feature));
                yield return StepData.MainStep(NewProjectStepTests, "5", StringRes.NewProjectStepFive, () => new TemplatesStepPage(TemplateType.Feature));
                yield return StepData.MainStep(NewProjectStepServices, "6", StringRes.NewProjectStepSix, () => new TemplatesStepPage(TemplateType.Feature));
            }
        }

        public override async Task InitializeAsync(string platform, string language)
        {
            WizardStatus.Title = $" ({GenContext.Current.ProjectName})";
            await base.InitializeAsync(platform, language);
            Navigation.OnFinish += OnFinish;
        }

        private void OnFinish(object sender, EventArgs e)
        {
            WizardShell.Current.Result = UserSelection.GetUserSelection();
        }

        public override bool IsSelectionEnabled(MetadataType metadataType)
        {
            bool result = false;
            if (!UserSelection.HasItemsAddedByUser)
            {
                result = true;
            }
            else
            {
                var vm = new QuestionDialogViewModel(metadataType);
                var questionDialog = new QuestionDialogWindow(vm);
                questionDialog.Owner = WizardShell.Current;
                questionDialog.ShowDialog();

                if (vm.Result == DialogResult.Accept)
                {
                    UserSelection.ResetUserSelection();
                    result = true;
                }
                else
                {
                    result = false;
                }
            }

            if (result == true)
            {
                StepsViewModels.Values.ToList().ForEach(vm => vm.ResetTemplatesCount());
            }

            return result;
        }

        public TemplateInfoViewModel GetTemplate(TemplateInfo templateInfo)
        {
            var groups = StepsViewModels[templateInfo.TemplateType].Groups;
            foreach (var group in groups)
            {
                var template = group.GetTemplate(templateInfo);
                if (template != null)
                {
                    return template;
                }
            }

            return null;
        }

        private async Task AddTemplateAsync(TemplateInfoViewModel selectedTemplate)
        {
            if (selectedTemplate.MultipleInstance || !UserSelection.IsTemplateAdded(selectedTemplate))
            {
                await UserSelection.AddAsync(TemplateOrigin.UserSelection, selectedTemplate);
            }
        }

        protected override async Task OnTemplatesAvailableAsync()
        {
            await ProjectType.LoadDataAsync(Platform);
            ShowNoContentPanel = !ProjectType.Items.Any();
        }

        public override async Task ProcessItemAsync(object item)
        {
            if (item is ProjectTypeMetaDataViewModel projectTypeMetaData)
            {
                ProjectType.Selected = projectTypeMetaData;
            }
            else if (item is FrameworkMetaDataViewModel frameworkMetaData)
            {
                Framework.Selected = frameworkMetaData;
            }
            else if (item is TemplateInfoViewModel template)
            {
                _selectedTemplate = template;
                await AddTemplateAsync(template);
            }
        }

        private async Task OnProjectTypeSelectedAsync()
        {
            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
            await Framework.LoadDataAsync(ProjectType.Selected.Name, Platform);
        }

        private async Task OnFrameworkSelectedAsync()
        {
            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
            BuildStepViewModel(TemplateType.Page, StringRes.AddPagesTitle);
            BuildStepViewModel(TemplateType.Feature, StringRes.AddFeaturesTitle);
            StepsViewModels.Values.ToList().ForEach(vm => vm.LoadData());
            await UserSelection.InitializeAsync(ProjectType.Selected.Name, Framework.Selected.Name, Platform, Language);
            WizardStatus.IsLoading = false;
        }

        private void BuildStepViewModel(TemplateType templateType, string title)
        {
            var hasTemplates = DataService.HasTemplatesFromType(templateType, Platform, ProjectType.Selected.Name, Framework.Selected.Name);
            var isStepAdded = StepsViewModels.ContainsKey(templateType);
            if (hasTemplates && !isStepAdded)
            {
                StepsViewModels.Add(templateType, new TemplatesStepViewModel(templateType, Platform, ProjectType.Selected.Name, Framework.Selected.Name, title));
            }
            else if (!hasTemplates && isStepAdded)
            {
                StepsViewModels.Remove(templateType);
            }
        }

        protected async Task OnRefreshTemplatesCacheAsync()
        {
            try
            {
                WizardStatus.IsLoading = true;
                UserSelection.ResetUserSelection();
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

        private void OnCompositionTool()
        {
            var compositionTool = new CompositionToolWindow(UserSelection.GetUserSelection());
            compositionTool.Owner = WizardShell.Current;
            compositionTool.ShowDialog();
        }

        private bool _showNoContentPanel;

        public bool ShowNoContentPanel
        {
            get => _showNoContentPanel;
            set => SetProperty(ref _showNoContentPanel, value);
        }
    }
}
