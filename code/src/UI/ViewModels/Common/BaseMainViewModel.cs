// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Extensions;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.Threading;

namespace Microsoft.Templates.UI.ViewModels.Common
{
    public abstract class BaseMainViewModel : Observable
    {
        private Window _mainView;

        private bool _canGoBack;

        private bool _canGoForward;

        private bool _canFinish;

        private bool _canCheckingUpdates;

        private bool _templatesAvailable;

        private bool _hasValidationErrors;

        private RelayCommand _cancelCommand;

        private RelayCommand _closeCommand;

        private RelayCommand _backCommand;

        private RelayCommand _nextCommand;

        private RelayCommand<string> _finishCommand;

        private RelayCommand _checkUpdatesCommand;

        private RelayCommand _refreshTemplatesCommand;

        private RelayCommand _refreshTemplatesCacheCommand;

        protected int CurrentStep { get; private set; }

        public WizardStatus WizardStatus { get; } = new WizardStatus();

        public RelayCommand CancelCommand => _cancelCommand ?? (_cancelCommand = new RelayCommand(OnCancel));

        public RelayCommand CloseCommand => _closeCommand ?? (_closeCommand = new RelayCommand(OnClose));

        public RelayCommand BackCommand => _backCommand ?? (_backCommand = new RelayCommand(OnGoBack, () => _canGoBack));

        public RelayCommand NextCommand => _nextCommand ?? (_nextCommand = new RelayCommand(OnNext, () => _templatesAvailable && !_hasValidationErrors && _canGoForward));

        public RelayCommand<string> FinishCommand => _finishCommand ?? (_finishCommand = new RelayCommand<string>(OnFinish, (parameter) => !_hasValidationErrors && _canFinish));

        public RelayCommand CheckUpdatesCommand => _checkUpdatesCommand ?? (_checkUpdatesCommand = new RelayCommand(
            () => SafeThreading.JoinableTaskFactory.RunAsync(async () => await OnCheckUpdatesAsync()),
            () => _canCheckingUpdates));

        public RelayCommand RefreshTemplatesCommand => _refreshTemplatesCommand ?? (_refreshTemplatesCommand = new RelayCommand(
            () => SafeThreading.JoinableTaskFactory.RunAsync(async () => await OnRefreshTemplatesAsync())));

        public RelayCommand RefreshTemplatesCacheCommand => _refreshTemplatesCacheCommand ?? (_refreshTemplatesCacheCommand = new RelayCommand(
            () => SafeThreading.JoinableTaskFactory.RunAsync(async () => await OnRefreshTemplatesAsync(true))));

        public bool CanForceRefreshTemplateCache
        {
            get
            {
                #if DEBUG
                    return true;
                #else
                    return false;
                #endif
            }
        }

        public BaseMainViewModel()
        {
        }

        public virtual void SetView(Window window)
        {
            _mainView = window;
            ResourceService.Initialize(_mainView);
        }

        protected abstract void OnCancel();

        protected abstract void OnClose();

        protected virtual void OnGoBack()
        {
            UpdateCanFinish(false);
            NavigationService.GoBack();
            CurrentStep--;
            UpdateCanGoBack(CurrentStep > 0);
        }

        protected virtual void OnNext()
        {
            UpdateCanGoBack(true);
            WizardStatus.IsOverlayBoxVisible = false;
            CurrentStep++;
        }

        protected virtual void OnFinish(string parameter)
        {
            _mainView.DialogResult = true;
            _mainView.Close();
        }

        private void UpdateCanGoBack(bool canGoBack)
        {
            _canGoBack = canGoBack;
            BackCommand.OnCanExecuteChanged();
        }

        public void UpdateCanGoForward(bool canGoForward)
        {
            _canGoForward = canGoForward;
            NextCommand.OnCanExecuteChanged();
        }

        public void UpdateCanFinish(bool canFinish)
        {
            _canFinish = canFinish;
            FinishCommand.OnCanExecuteChanged();
            WizardStatus.ShowFinishButton = canFinish;
        }

        private void UpdateCanCheckUpdates(bool value)
        {
            _canCheckingUpdates = value;
            CheckUpdatesCommand.OnCanExecuteChanged();
        }

        private void UpdateHasValidationErrors(bool value)
        {
            _hasValidationErrors = value;
            NextCommand.OnCanExecuteChanged();
            FinishCommand.OnCanExecuteChanged();
        }

        public void SetValidationErrors(string errorMessage, StatusType statusType = StatusType.Error)
        {
            WizardStatus.SetStatus(new StatusViewModel(statusType, errorMessage));
            UpdateHasValidationErrors(true);
        }

        public void CleanStatus(bool cleanValidationError = false)
        {
            WizardStatus.ClearStatus();
            if (cleanValidationError)
            {
                UpdateHasValidationErrors(false);
            }
        }

        protected abstract Task OnTemplatesAvailableAsync();

        protected abstract Task OnNewTemplatesAvailableAsync();

        protected async Task BaseInitializeAsync()
        {
            GenContext.ToolBox.Repo.Sync.SyncStatusChanged += SyncSyncStatusChanged;
            try
            {
                await GenContext.ToolBox.Repo.SynchronizeAsync();

                WizardStatus.TemplatesVersion = GenContext.ToolBox.TemplatesVersion;
                WizardStatus.WizardVersion = GenContext.ToolBox.WizardVersion;
            }
            catch (Exception ex)
            {
                WizardStatus.SetStatus(StatusViewModel.Information(StringRes.ErrorSync));

                await AppHealth.Current.Error.TrackAsync(ex.ToString());
                await AppHealth.Current.Exception.TrackAsync(ex);
            }
            finally
            {
                WizardStatus.IsLoading = GenContext.ToolBox.Repo.SyncInProgress;
                UpdateCanCheckUpdates(!GenContext.ToolBox.Repo.SyncInProgress);
            }
        }

        private async void SyncSyncStatusChanged(object sender, SyncStatusEventArgs status)
        {
            SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                    await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                    WizardStatus.SetStatus(status.Status.GetStatusViewModel(status.Version));
            });

            if (status.Status == SyncStatus.Ready || status.Status == SyncStatus.Updated)
            {
                WizardStatus.TemplatesVersion = GenContext.ToolBox.Repo.TemplatesVersion;

                UpdateTemplatesAvailable(true);
                await OnTemplatesAvailableAsync();
                WizardStatus.IsLoading = false;
                UpdateCanCheckUpdates(true);
            }

            if (status.Status == SyncStatus.Acquired)
            {
                WizardStatus.NewVersionAvailable = true;
            }
        }

        public void UnsuscribeEventHandlers() => GenContext.ToolBox.Repo.Sync.SyncStatusChanged -= SyncSyncStatusChanged;

        private async Task OnRefreshTemplatesAsync(bool force = false)
        {
            try
            {
                await GenContext.ToolBox.Repo.RefreshAsync(force);
                WizardStatus.TemplatesVersion = GenContext.ToolBox.TemplatesVersion;
                await OnNewTemplatesAvailableAsync();
                ResetWizardSteps();
                WizardStatus.NewVersionAvailable = false;
                WizardStatus.SetStatus(StatusViewModel.Information(StringRes.StatusUpdated, true, 5));
            }
            catch (Exception ex)
            {
                WizardStatus.SetStatus(StatusViewModel.Information(StringRes.ErrorSyncRefresh));

                await AppHealth.Current.Error.TrackAsync(ex.ToString());
                await AppHealth.Current.Exception.TrackAsync(ex);
            }
            finally
            {
                WizardStatus.IsLoading = GenContext.ToolBox.Repo.SyncInProgress;
            }
        }

        public void ResetWizardSteps()
        {
            CurrentStep = 0;
            UpdateCanGoBack(false);
            UpdateCanGoForward(true);
            UpdateCanFinish(false);
        }

        private async Task OnCheckUpdatesAsync()
        {
            try
            {
                UpdateCanCheckUpdates(false);
                await GenContext.ToolBox.Repo.CheckForUpdatesAsync();
            }
            catch (Exception ex)
            {
                WizardStatus.SetStatus(StatusViewModel.Information(StringRes.ErrorSyncRefresh));
                await AppHealth.Current.Error.TrackAsync(ex.ToString());
                await AppHealth.Current.Exception.TrackAsync(ex);
            }
            finally
            {
                WizardStatus.IsLoading = GenContext.ToolBox.Repo.SyncInProgress;
                UpdateCanCheckUpdates(!GenContext.ToolBox.Repo.SyncInProgress);
            }
        }

        private void UpdateTemplatesAvailable(bool value)
        {
            _templatesAvailable = value;
            NextCommand.OnCanExecuteChanged();
        }
    }
}
