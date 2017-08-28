// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Controls;
using Microsoft.Templates.UI.Extensions;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.Services;

namespace Microsoft.Templates.UI.ViewModels.Common
{
    public abstract class BaseMainViewModel : Observable
    {
        private Window _mainView;
        protected bool _canFinish;
        protected bool _canGoBack;
        protected bool _canGoForward;
        protected bool _hasValidationErrors;
        protected bool _templatesAvailable;
        protected bool _canCheckingUpdates;

        protected StatusViewModel _status = StatusViewModel.EmptyStatus;
        public StatusViewModel Status
        {
            get => _status;
            private set
            {
                SetProperty(ref _status, value);
                HasStatus = value != null && value.Status != StatusType.Empty;
            }
        }

        protected StatusViewModel _overlayStatus = StatusViewModel.EmptyStatus;
        public StatusViewModel OverlayStatus
        {
            get => _overlayStatus;
            private set => SetProperty(ref _overlayStatus, value);
        }

        protected bool _hasStatus;
        public bool HasStatus
        {
            get => _hasStatus;
            private set => SetProperty(ref _hasStatus, value);
        }

        protected bool _isOverlayBoxVisible;
        public bool IsOverlayBoxVisible
        {
            get => _isOverlayBoxVisible;
            set => SetProperty(ref _isOverlayBoxVisible, value);
        }

        protected bool _hasOverlayBox = true;
        public bool HasOverlayBox
        {
            get => _hasOverlayBox;
            protected set => SetProperty(ref _hasOverlayBox, value);
        }

        protected string _wizardVersion;
        public string WizardVersion
        {
            get => _wizardVersion;
            set => SetProperty(ref _wizardVersion, value);
        }

        protected string _templatesVersion;
        public string TemplatesVersion
        {
            get => _templatesVersion;
            set => SetProperty(ref _templatesVersion, value);
        }

        protected string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        protected bool _newVersionAvailable;
        public bool NewVersionAvailable
        {
            get => _newVersionAvailable;
            set => SetProperty(ref _newVersionAvailable, value);
        }

        protected bool _isLoading = true;
        public bool IsLoading
        {
            get => _isLoading;
            private set => SetProperty(ref _isLoading, value);
        }

        protected Visibility _infoShapeVisibility = Visibility.Collapsed;
        public Visibility InfoShapeVisibility
        {
            get => _infoShapeVisibility;
            set => SetProperty(ref _infoShapeVisibility, value);
        }

        protected bool _hasContent;
        public bool HasContent
        {
            get => _hasContent;
            set => SetProperty(ref _hasContent, value);
        }

        protected bool _showFinishButton;
        public bool ShowFinishButton
        {
            get => _showFinishButton;
            set => SetProperty(ref _showFinishButton, value);
        }

        #region Commands
        private RelayCommand _showOverlayMenuCommand;
        protected RelayCommand _closeCommand;
        protected RelayCommand _cancelCommand;
        protected RelayCommand _goBackCommand;
        protected RelayCommand _nextCommand;
        protected RelayCommand<string> _finishCommand;

        protected RelayCommand _checkUpdatesCommand;
        protected RelayCommand _refreshTemplatesCommand;

        public RelayCommand CancelCommand => _cancelCommand ?? (_cancelCommand = new RelayCommand(OnCancel));
        public RelayCommand CloseCommand => _closeCommand ?? (_closeCommand = new RelayCommand(OnClose));
        public RelayCommand BackCommand => _goBackCommand ?? (_goBackCommand = new RelayCommand(OnGoBack, () => _canGoBack));
        public RelayCommand NextCommand => _nextCommand ?? (_nextCommand = new RelayCommand(OnNext, () => _templatesAvailable && !_hasValidationErrors && _canGoForward));
        public RelayCommand ShowOverlayMenuCommand => _showOverlayMenuCommand ?? (_showOverlayMenuCommand = new RelayCommand(() => IsOverlayBoxVisible = !IsOverlayBoxVisible));
        public RelayCommand<string> FinishCommand => _finishCommand ?? (_finishCommand = new RelayCommand<string>(OnFinish, CanFinish));
        public RelayCommand CheckUpdatesCommand => _checkUpdatesCommand ?? (_checkUpdatesCommand = new RelayCommand(async () => await OnCheckUpdatesAsync(), CanCheckUpdates));
        public RelayCommand RefreshTemplatesCommand => _refreshTemplatesCommand ?? (_refreshTemplatesCommand = new RelayCommand(async () => await OnRefreshTemplatesAsync()));
        #endregion

        public BaseMainViewModel(Window mainView)
        {
            _mainView = mainView;
        }

        protected abstract void OnCancel();

        protected abstract void OnClose();

        protected virtual void OnNext()
        {
            _canGoBack = true;
            IsOverlayBoxVisible = false;
            BackCommand.OnCanExecuteChanged();
            ShowFinishButton = true;
        }

        protected abstract Task OnTemplatesAvailableAsync();

        protected abstract Task OnNewTemplatesAvailableAsync();

        public abstract UserSelection CreateUserSelection();

        public void SetValidationErrors(string errorMessage, StatusType statusType = StatusType.Error)
        {
            SetStatus(new StatusViewModel(statusType, errorMessage));
            _hasValidationErrors = true;
            FinishCommand.OnCanExecuteChanged();
        }

        public void UpdateCanFinish(bool canFinish)
        {
            _canFinish = canFinish;
            FinishCommand.OnCanExecuteChanged();
        }

        public void CleanStatus(bool cleanValidationError = false)
        {
            if (Status.CanBeCleaned)
            {
                SetStatus(StatusViewModel.EmptyStatus);
            }
            if (cleanValidationError)
            {
                _hasValidationErrors = false;
                NextCommand.OnCanExecuteChanged();
                FinishCommand.OnCanExecuteChanged();
            }
        }

        public void EnableGoForward()
        {
            _canGoForward = true;
            NextCommand.OnCanExecuteChanged();
        }
        public virtual void UnsuscribeEventHandlers()
        {
            GenContext.ToolBox.Repo.Sync.SyncStatusChanged -= SyncSyncStatusChanged;
        }

        public void TryHideOverlayBox(FrameworkElement element)
        {
            if (element is OverlayBox)
            {
                return;
            }
            else if (element?.Tag != null && element.Tag.ToString() == "AllowOverlay")
            {
                return;
            }

            IsOverlayBoxVisible = false;
        }

        protected virtual void OnGoBack()
        {
            NavigationService.GoBack();
            _canGoBack = false;
            _canFinish = false;
            BackCommand.OnCanExecuteChanged();

            ShowFinishButton = false;
        }
        protected virtual bool CanFinish(string parameter)
        {
            if (_hasValidationErrors || !_canFinish)
            {
                return false;
            }
            return true;
        }

        private bool CanCheckUpdates()
        {
            return _canCheckingUpdates;
        }

        private void SetCanCheckUpdates(bool value)
        {
            _canCheckingUpdates = value;
            CheckUpdatesCommand.OnCanExecuteChanged();
        }
        protected virtual void OnFinish(string parameter)
        {
            _mainView.DialogResult = true;
            _mainView.Close();
        }

        private async Task OnRefreshTemplatesAsync()
        {
            try
            {
                await GenContext.ToolBox.Repo.RefreshAsync();
                TemplatesVersion = GenContext.ToolBox.TemplatesVersion;
                await OnNewTemplatesAvailableAsync();
                NewVersionAvailable = false;
                SetStatus(StatusViewModel.Information(StringRes.StatusUpdated, true, 5));
            }
            catch (Exception ex)
            {
                SetStatus(StatusViewModel.Information(StringRes.ErrorSyncRefresh));

                await AppHealth.Current.Error.TrackAsync(ex.ToString());
                await AppHealth.Current.Exception.TrackAsync(ex);
            }
            finally
            {
                IsLoading = GenContext.ToolBox.Repo.SyncInProgress;
            }
        }

        private async Task OnCheckUpdatesAsync()
        {
            try
            {
                SetCanCheckUpdates(false);
                await GenContext.ToolBox.Repo.CheckForUpdatesAsync();
            }
            catch (Exception ex)
            {
                SetStatus(StatusViewModel.Information(StringRes.ErrorSyncRefresh));
                await AppHealth.Current.Error.TrackAsync(ex.ToString());
                await AppHealth.Current.Exception.TrackAsync(ex);
            }
            finally
            {
                IsLoading = GenContext.ToolBox.Repo.SyncInProgress;
                SetCanCheckUpdates(!GenContext.ToolBox.Repo.SyncInProgress);
            }
        }

        protected async Task BaseInitializeAsync()
        {
            GenContext.ToolBox.Repo.Sync.SyncStatusChanged += SyncSyncStatusChanged;
            try
            {
                await GenContext.ToolBox.Repo.SynchronizeAsync();

                TemplatesVersion = GenContext.ToolBox.TemplatesVersion;
                WizardVersion = GenContext.ToolBox.WizardVersion;
            }
            catch (Exception ex)
            {
                SetStatus(StatusViewModel.Information(StringRes.ErrorSync));

                await AppHealth.Current.Error.TrackAsync(ex.ToString());
                await AppHealth.Current.Exception.TrackAsync(ex);
            }
            finally
            {
                IsLoading = GenContext.ToolBox.Repo.SyncInProgress;
                SetCanCheckUpdates(!GenContext.ToolBox.Repo.SyncInProgress);
            }
        }

        private string GetStatusText(SyncStatus status)
        {
            switch (status)
            {
                case SyncStatus.Updating:
                    return StringRes.StatusUpdating;
                case SyncStatus.Updated:
                    return StringRes.StatusUpdated;
                case SyncStatus.Acquiring:
                    return StringRes.StatusAcquiring;
                case SyncStatus.Acquired:
                    return StringRes.StatusAcquired;
                case SyncStatus.Preparing:
                    return StringRes.StatusPreparing;
                case SyncStatus.Prepared:
                    return StringRes.StatusPrepared;
                case SyncStatus.NewVersionAvailable:
                    return StringRes.StatusNewVersionAvailable;
                default:
                    return string.Empty;
            }
        }

        private int GetStatusHideSeconds(SyncStatus status)
        {
            switch (status)
            {
                case SyncStatus.Updating:
                    return 5;
                case SyncStatus.Updated:
                    return 5;
                case SyncStatus.Acquiring:
                    return 0;
                case SyncStatus.Acquired:
                    return 5;
                case SyncStatus.Preparing:
                    return 5;
                case SyncStatus.Prepared:
                    return 5;
                case SyncStatus.NewVersionAvailable:
                    return 5;
                default:
                    return 5;
            }
        }

        public void SetStatus(StatusViewModel status)
        {
            if (status.Status == StatusType.Empty)
            {
                OverlayStatus = status;
                Status = status;
            }
            else
            {
                if (status.Status == StatusType.Information && IsOverlayBoxVisible)
                {
                    OverlayStatus = status;
                }
                else
                {
                    Status = status;
                }
            }
        }

        private async void SyncSyncStatusChanged(object sender, SyncStatusEventArgs status)
        {
            SetStatus(StatusViewModel.Information(GetStatusText(status.Status), true, GetStatusHideSeconds(status.Status)));

            if (status.Status == SyncStatus.Updated)
            {
                TemplatesVersion = GenContext.ToolBox.Repo.TemplatesVersion;
                CleanStatus();

                _templatesAvailable = true;
                await OnTemplatesAvailableAsync();
                NextCommand.OnCanExecuteChanged();
                IsLoading = false;
                SetCanCheckUpdates(true);
            }
            if (status.Status == SyncStatus.OverVersion)
            {
                _mainView.Dispatcher.Invoke(() =>
                {
                    SetStatus(StatusViewModel.Warning(StringRes.StatusOverVersionContent));
                });
            }

            if (status.Status == SyncStatus.OverVersionNoContent)
            {
                _mainView.Dispatcher.Invoke(() =>
                {
                    SetStatus(StatusViewModel.Error(StringRes.StatusOverVersionNoContent));
                    _templatesAvailable = false;
                    NextCommand.OnCanExecuteChanged();
                });
            }

            if (status.Status == SyncStatus.UnderVersion)
            {
                _mainView.Dispatcher.Invoke(() =>
                {
                    SetStatus(StatusViewModel.Error(StringRes.StatusLowerVersionContent));
                    _templatesAvailable = false;
                    NextCommand.OnCanExecuteChanged();
                });
            }

            if (status.Status == SyncStatus.NewVersionAvailable)
            {
                NewVersionAvailable = true;
            }
        }
    }
}
