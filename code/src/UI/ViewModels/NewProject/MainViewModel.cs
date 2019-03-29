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
        private RelayCommand _refreshTemplatesCacheCommand;
        private RelayCommand _compositionToolCommand;

        private TemplateInfoViewModel _selectedTemplate;

        public static MainViewModel Instance { get; private set; }

        public ProjectTypeViewModel ProjectType { get; } = new ProjectTypeViewModel(() => Instance.IsSelectionEnabled(MetadataType.ProjectType), () => Instance.OnProjectTypeSelectedAsync());

        public FrameworkViewModel Framework { get; } = new FrameworkViewModel(() => Instance.IsSelectionEnabled(MetadataType.Framework), () => Instance.OnFrameworkSelectedAsync());

        public AddPagesViewModel AddPages { get; } = new AddPagesViewModel();

        public AddFeaturesViewModel AddFeatures { get; } = new AddFeaturesViewModel();

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
            : base(mainView, provider)
        {
            Instance = this;
            ValidationService.Initialize(UserSelection.GetNames, UserSelection.GetPageNames);
        }

        public override async Task InitializeAsync(string platform, string language)
        {
            WizardStatus.Title = $" ({GenContext.Current.ProjectName})";
            await base.InitializeAsync(platform, language);
        }

        protected override void OnCancel() => WizardShell.Current.Close();

        protected override void OnFinish()
        {
            WizardShell.Current.Result = UserSelection.GetUserSelection();
            base.OnFinish();
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
                AddPages.ResetUserSelection();
                AddFeatures.ResetTemplatesCount();
            }

            return result;
        }

        public TemplateInfoViewModel GetTemplate(TemplateInfo templateInfo)
        {
            var groups = templateInfo.TemplateType == TemplateType.Page ? AddPages.Groups : AddFeatures.Groups;
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

        protected override IEnumerable<Step> GetSteps()
        {
            yield return new Step(0, StringRes.NewProjectStepOne, () => new ProjectTypePage(), true, true);
            yield return new Step(1, StringRes.NewProjectStepTwo, () => new FrameworkPage());
            yield return new Step(2, StringRes.NewProjectStepThree, () => new AddPagesPage());
            yield return new Step(3, StringRes.NewProjectStepFour, () => new AddFeaturesPage());
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
            AddPages.LoadData(Platform, ProjectType.Selected.Name, Framework.Selected.Name);
            AddFeatures.LoadData(Platform, ProjectType.Selected.Name, Framework.Selected.Name);
            await UserSelection.InitializeAsync(ProjectType.Selected.Name, Framework.Selected.Name, Platform, Language);
            WizardStatus.IsLoading = false;
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
