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
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.UI.Controls;
using Microsoft.Templates.UI.Extensions;
using Microsoft.Templates.UI.Mvvm;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.Threading;

namespace Microsoft.Templates.UI.ViewModels.Common
{
    public abstract class BaseMainViewModel : Observable
    {
        public static BaseMainViewModel BaseInstance { get; private set; }

        private int _step;
        private int _origStep;
        private bool _canGoBack = false;
        private bool _canGoForward = true;
        private bool _canFinish;

        private RelayCommand _cancelCommand;
        private RelayCommand _goBackCommand;
        private RelayCommand _goForwardCommand;
        private RelayCommand _finishCommand;

        protected Window MainView { get; private set; }

        protected string Language { get; private set; }

        protected string Platform { get; private set; }

        public int Step
        {
            get => _step;
            private set => SetStepAsync(value).FireAndForget();
        }

        public ObservableCollection<Step> Steps { get; }

        public RelayCommand CancelCommand => _cancelCommand ?? (_cancelCommand = new RelayCommand(OnCancel));

        public RelayCommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new RelayCommand(() => Step--, () => _canGoBack && !WizardStatus.IsBusy));

        public RelayCommand GoForwardCommand => _goForwardCommand ?? (_goForwardCommand = new RelayCommand(() => Step++, () => _canGoForward && !WizardStatus.IsBusy));

        public RelayCommand FinishCommand => _finishCommand ?? (_finishCommand = new RelayCommand(OnFinish, () => _canFinish && !WizardStatus.IsBusy));

        public WizardStatus WizardStatus { get; }

        public SystemService SystemService { get; }

        public UIStylesService StylesService { get; }

        public BaseMainViewModel(Window mainView, BaseStyleValuesProvider provider, bool canFinish = true)
        {
            BaseInstance = this;
            MainView = mainView;
            _canFinish = canFinish;
            Steps = new ObservableCollection<Step>();
            SystemService = new SystemService();
            WizardStatus = new WizardStatus();
            StylesService = new UIStylesService(provider);
            ResourcesService.Instance.Initialize(mainView);
        }

        public abstract bool IsSelectionEnabled(MetadataType metadataType);

        public abstract Task ProcessItemAsync(object item);

        protected abstract void OnCancel();

        protected abstract Task OnTemplatesAvailableAsync();

        protected abstract IEnumerable<Step> GetSteps();

        public void RefreshStep(object navigatedPage)
        {
            var step = Steps.FirstOrDefault(s => s.Equals(navigatedPage.GetType()));
            if (step != null)
            {
                SetStepAsync(step.Index, false).FireAndForget();
            }
        }

        public void UnsubscribeEventHandlers()
        {
            GenContext.ToolBox.Repo.Sync.SyncStatusChanged -= OnSyncStatusChanged;
            SystemService.UnsubscribeEventHandlers();
            StylesService.UnsubscribeEventHandlers();
        }

        public async Task<bool> IsStepAvailableAsync() => await IsStepAvailableAsync(Step);

        public async Task SetStepAsync(int step, bool navigate = true)
        {
            _origStep = _step;
            if (step != _step)
            {
                _step = step;
            }

            if (await IsStepAvailableAsync(step))
            {
                OnPropertyChanged(nameof(Step));
                UpdateStep(navigate);
            }
            else
            {
                DispatcherService.BeginInvoke(() =>
                {
                    _step = _origStep;
                    OnPropertyChanged(nameof(Step));
                });
            }
        }

        public Step GetCurrentStep() => Steps.FirstOrDefault(step => step.Equals(Step));

        public virtual async Task InitializeAsync(string platform, string language)
        {
            Platform = platform;
            Language = language;
            Steps.Clear();
            foreach (var step in GetSteps())
            {
                Steps.Add(step);
            }

            GenContext.ToolBox.Repo.Sync.SyncStatusChanged += OnSyncStatusChanged;
            SystemService.Initialize();
            try
            {
                await GenContext.ToolBox.Repo.SynchronizeAsync();
            }
            catch (Exception ex)
            {
                await AppHealth.Current.Error.TrackAsync(ex.ToString());
                await AppHealth.Current.Exception.TrackAsync(ex);
            }
        }

        protected virtual async Task<bool> IsStepAvailableAsync(int step)
        {
            await Task.CompletedTask;
            return !WizardStatus.HasValidationErrors;
        }

        protected virtual void UpdateStep(bool navigate)
        {
            var compleatedSteps = Steps.Where(s => s.IsPrevious(Step));
            foreach (var step in compleatedSteps)
            {
                step.Completed = true;
            }

            foreach (var step in Steps)
            {
                step.IsSelected = false;
            }

            var selectedStep = GetCurrentStep();
            if (selectedStep != null)
            {
                selectedStep.IsSelected = true;
                if (navigate)
                {
                    NavigationService.NavigateSecondaryFrame(selectedStep.GetPage());
                }
            }

            _canGoBack = Step > 0;
            _canGoForward = Step < Steps.Count - 1;
        }

        protected virtual void OnFinish()
        {
            if (MainView != null)
            {
                MainView.DialogResult = true;
                MainView?.Close();
            }
        }

        protected void SetCanFinish(bool canFinish)
        {
            _canFinish = canFinish;
        }

        private async void OnSyncStatusChanged(object sender, SyncStatusEventArgs args)
        {
            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
            WizardStatus.SetVersions();

            var notification = args.GetNotification();
            if (notification?.Category == Category.TemplatesSync)
            {
                await NotificationsControl.AddOrUpdateNotificationAsync(notification);
            }
            else
            {
                await NotificationsControl.AddNotificationAsync(notification);
            }

            if (args.Status == SyncStatus.NoUpdates || args.Status == SyncStatus.Ready)
            {
                NotificationsControl.RemoveNotification();
            }

            if (args.Status == SyncStatus.Updated || args.Status == SyncStatus.Ready)
            {
                await OnTemplatesAvailableAsync();
            }
        }

        private void OnResetSelection(object sender, EventArgs e)
        {
            _step = _origStep;
            OnPropertyChanged("Step");
        }
    }
}
