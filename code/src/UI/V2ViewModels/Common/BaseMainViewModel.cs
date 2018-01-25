// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Threading;
using Microsoft.Templates.UI.V2Controls;
using Microsoft.Templates.UI.V2Extensions;
using Microsoft.Templates.UI.V2Services;

namespace Microsoft.Templates.UI.V2ViewModels.Common
{
    public abstract class BaseMainViewModel : Observable
    {
        public static BaseMainViewModel Instance;
        private Window _mainView;
        private int _step;
        private int _origStep;
        private bool _canGoBack = false;
        private bool _canGoForward = true;

        private RelayCommand _cancelCommand;
        private RelayCommand _goBackCommand;
        private RelayCommand _goForwardCommand;
        private RelayCommand _finishCommand;
        private DispatcherTimer _setStepTimer;
        private DispatcherTimer _resetStepTimer;

        protected string Language { get; private set; }

        public int Step
        {
            get => _step;
            set => SetStep(value);
        }

        private async void SetStep(int step)
        {
            _origStep = _step;
            if (step != _step)
            {
                _step = step;
            }

            if (await IsStepAvailableAsync(step))
            {
                OnPropertyChanged("Step");
                UpdateStep();
            }
            else
            {
                _resetStepTimer.Start();
            }
        }

        public ObservableCollection<Step> Steps { get; } = new ObservableCollection<Step>();

        public RelayCommand CancelCommand => _cancelCommand ?? (_cancelCommand = new RelayCommand(OnCancel));

        public RelayCommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new RelayCommand(() => Step--, () => _canGoBack && !WizardStatus.IsBusy));

        public RelayCommand GoForwardCommand => _goForwardCommand ?? (_goForwardCommand = new RelayCommand(() => Step++, () => _canGoForward && !WizardStatus.IsBusy));

        public RelayCommand FinishCommand => _finishCommand ?? (_finishCommand = new RelayCommand(OnFinish, () => !WizardStatus.IsBusy));

        public WizardStatus WizardStatus { get; } = new WizardStatus();

        protected virtual async Task<bool> IsStepAvailableAsync(int step)
        {
            await Task.CompletedTask;
            return true;
        }

        protected virtual void UpdateStep()
        {
            var currentStep = Steps.FirstOrDefault(s => s.Index == Step);
            if (currentStep != null)
            {
                currentStep.Completed = true;
            }
        }

        protected abstract void OnCancel();

        protected abstract Task OnTemplatesAvailableAsync();

        protected abstract IEnumerable<Step> GetSteps();

        public abstract void ProcessItem(object item);

        public BaseMainViewModel(Window mainView)
        {
            Instance = this;
            _mainView = mainView;
            ResourcesService.Instance.Initialize(mainView);
            WizardStatus.IsBusyChanged += IsBusyChanged;

            _resetStepTimer = new DispatcherTimer(DispatcherPriority.ContextIdle, Application.Current.Dispatcher);
            _resetStepTimer.Interval = TimeSpan.FromMilliseconds(1);
            _resetStepTimer.Tick += OnResetSelection;
        }

        protected virtual void OnFinish()
        {
            _mainView.DialogResult = true;
            _mainView.Close();
        }

        protected void SetCanGoBack(bool canGoBack)
        {
            _canGoBack = canGoBack;
            GoBackCommand.OnCanExecuteChanged();
        }

        protected void SetCanGoForward(bool canGoForward)
        {
            _canGoForward = canGoForward;
            GoForwardCommand.OnCanExecuteChanged();
        }

        protected virtual void IsBusyChanged(object sender, bool e)
        {
            FinishCommand.OnCanExecuteChanged();
            GoBackCommand.OnCanExecuteChanged();
            GoForwardCommand.OnCanExecuteChanged();
        }

        public virtual async Task InitializeAsync(string language)
        {
            Language = language;
            Steps.Clear();
            foreach (var step in GetSteps())
            {
                Steps.Add(step);
            }

            GenContext.ToolBox.Repo.Sync.SyncStatusChanged += OnSyncStatusChanged;
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

        private async void OnSyncStatusChanged(object sender, SyncStatusEventArgs args)
        {
            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
            WizardStatus.SetVersions();
            await NotificationsControl.Instance.AddNotificationAsync(args.GetNotification());

            System.Diagnostics.Debug.WriteLine(GenContext.ToolBox.TemplatesVersion);
            if (args.Status == SyncStatus.Updated || args.Status == SyncStatus.Ready)
            {
                await OnTemplatesAvailableAsync();
            }
            else if (args.Status == SyncStatus.Acquired)
            {
                // TODO: Turn on the light that indicates that there are templates updates.
            }
        }

        private void OnResetSelection(object sender, EventArgs e)
        {
            _step = _origStep;
            OnPropertyChanged("Step");
            _resetStepTimer.Stop();
        }
    }
}
