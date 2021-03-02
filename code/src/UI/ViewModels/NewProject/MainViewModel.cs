// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.Controls;
using Microsoft.Templates.UI.Extensions;
using Microsoft.Templates.UI.Mvvm;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.Threading;
using Microsoft.Templates.UI.ViewModels.Common;
using Microsoft.Templates.UI.Views.Common;
using Microsoft.Templates.UI.Views.NewProject;
using Newtonsoft.Json;

namespace Microsoft.Templates.UI.ViewModels.NewProject
{
    public class MainViewModel : BaseMainViewModel
    {
        public const string NewProjectStepProjectType = "01ProjectType";
        public const string NewProjectStepFramework = "02Framework";

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
            Navigation.OnFinish += OnFinish;
        }

        public override void UnsubscribeEventHandlers()
        {
            base.UnsubscribeEventHandlers();
            Navigation.OnFinish -= OnFinish;
            UserSelection.UnsubscribeEventHandlers();
        }

        private static IEnumerable<StepData> NewProjectSteps
        {
            get
            {
                yield return StepData.MainStep(NewProjectStepProjectType, "1", StringRes.NewProjectStepProjectType, () => new ProjectTypePage(), true, true);
                yield return StepData.MainStep(NewProjectStepFramework, "2", StringRes.NewProjectStepDesignPattern, () => new FrameworkPage());
            }
        }

        public override void Initialize(UserSelectionContext context)
        {
            switch (context.Platform)
            {
                case Platforms.Uwp:
                    WizardStatus.Title = $"{StringRes.NewProjectTitleUWP} ({GenContext.Current.ProjectName})";
                    break;
                case Platforms.Wpf:
                    WizardStatus.Title = $"{StringRes.NewProjectTitleWPF} ({GenContext.Current.ProjectName})";
                    break;
                case Platforms.WinUI:
                    context.PropertyBag.TryGetValue("appmodel", out var appModel);
                    switch (appModel)
                    {
                        case AppModels.Desktop:
                            WizardStatus.Title = $"{StringRes.NewProjectTitleWinUIDesktop} ({GenContext.Current.ProjectName})";
                            break;
                        case AppModels.Uwp:
                            WizardStatus.Title = $"{StringRes.NewProjectTitleWinUIUWP} ({GenContext.Current.ProjectName})";
                            break;
                    }

                    break;
                default:
                    break;
            }

            base.Initialize(context);
        }

        private void OnFinish(object sender, EventArgs e)
        {
            WizardShell.Current.Result = UserSelection.GetUserSelection();
        }

        public override bool IsSelectionEnabled(MetadataType metadataType)
        {
            if (WizardStatus.HasValidationErrors)
            {
                return false;
            }

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
            if (!selectedTemplate.Disabled && selectedTemplate.CanBeAdded)
            {
                await UserSelection.AddAsync(TemplateOrigin.UserSelection, selectedTemplate);
            }
        }

        public override async Task OnTemplatesAvailableAsync()
        {
            ValidationService.Initialize(UserSelection.GetNames, UserSelection.GetPageNames);
            await ProjectType.LoadDataAsync(Context);
            ShowNoContentPanel = !ProjectType.Items.Any();
        }

        public override async Task ProcessItemAsync(object item)
        {
            if (WizardStatus.HasValidationErrors)
            {
                return;
            }

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

            Context.ProjectType = ProjectType.Selected.Name;
            await Framework.LoadDataAsync(Context);
        }

        private async Task OnFrameworkSelectedAsync()
        {
            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();

            Context.FrontEndFramework = Framework.Selected.Name;

            UserSelection.Initialize(Context);

            await BuildStepViewModelAsync(TemplateType.Page);
            await BuildStepViewModelAsync(TemplateType.Feature);
            await BuildStepViewModelAsync(TemplateType.Service);
            await BuildStepViewModelAsync(TemplateType.Testing);

            await UserSelection.AddLayoutTemplatesAsync();

            WizardStatus.IsLoading = false;
        }

        private async Task BuildStepViewModelAsync(TemplateType templateType)
        {
            var hasTemplates = DataService.HasTemplatesFromType(templateType, Context);
            var stepId = templateType.GetNewProjectStepId();
            var isStepAdded = StepsViewModels.ContainsKey(templateType);
            if (hasTemplates)
            {
                if (!isStepAdded)
                {
                    var stepTitle = templateType.GetNewProjectStepTitle();
                    var pageTitle = templateType.GetStepPageTitle();
                    var step = new TemplatesStepViewModel(templateType, Context, pageTitle);
                    step.LoadData();
                    StepsViewModels.Add(templateType, step);
                    WizardNavigation.Current.AddNewStep(stepId, stepTitle, () => new TemplatesStepPage(templateType));
                }
                else
                {
                    var step = StepsViewModels[templateType];
                    step.ResetData(Context.ProjectType, Context.FrontEndFramework);
                }
            }
            else if (!hasTemplates && isStepAdded)
            {
                StepsViewModels.Remove(templateType);
                await WizardNavigation.Current.RemoveStepAsync(stepId);
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
