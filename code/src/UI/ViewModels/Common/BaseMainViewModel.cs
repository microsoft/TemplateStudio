// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

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
        protected bool _templatesReady;
        protected bool _canGoBack;
        protected bool _canGoForward;
        protected bool _hasValidationErrors;
        protected bool _templatesAvailable;

        protected StatusViewModel _status = StatusControl.EmptyStatus;
        public StatusViewModel Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        protected bool _isOverlayBoxVisible;
        public bool IsOverlayBoxVisible
        {
            get => _isOverlayBoxVisible;
            private set => SetProperty(ref _isOverlayBoxVisible, value);
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
        public RelayCommand CheckUpdatesCommand => _checkUpdatesCommand ?? (_checkUpdatesCommand = new RelayCommand(OnCheckUpdates));
        public RelayCommand RefreshTemplatesCommand => _refreshTemplatesCommand ?? (_refreshTemplatesCommand = new RelayCommand(OnRefreshTemplates));
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
        protected abstract void OnTemplatesAvailable();
        protected abstract void OnNewTemplatesAvailable();
        protected abstract UserSelection CreateUserSelection();

        public void SetValidationErrors(string errorMessage, StatusType statusType = StatusType.Error)
        {
            Status = new StatusViewModel(statusType, errorMessage);
            _hasValidationErrors = true;
            FinishCommand.OnCanExecuteChanged();
        }

        public void SetTemplatesReadyForProjectCreation()
        {
            _templatesReady = true;
            FinishCommand.OnCanExecuteChanged();
        }

        public void CleanStatus(bool cleanValidationError = false)
        {
            Status = StatusControl.EmptyStatus;
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

        protected virtual void OnGoBack()
        {
            NavigationService.GoBack();
            _canGoBack = false;
            _templatesReady = false;
            BackCommand.OnCanExecuteChanged();

            ShowFinishButton = false;
        }
        protected virtual bool CanFinish(string parameter)
        {
            if (_hasValidationErrors || !_templatesReady)
            {
                return false;
            }
            CleanStatus();
            return true;
        }
        protected virtual void OnFinish(string parameter)
        {
            _mainView.DialogResult = true;
            _mainView.Close();
        }

        private async void OnRefreshTemplates()
        {
            try
            {
                await GenContext.ToolBox.Repo.RefreshAsync();
                TemplatesVersion = GenContext.ToolBox.TemplatesVersion;
                OnNewTemplatesAvailable();
                NewVersionAvailable = false;
                IsOverlayBoxVisible = false;
            }
            catch (Exception ex)
            {
                Status = new StatusViewModel(StatusType.Information, StringRes.ErrorSyncRefresh, true);

                await AppHealth.Current.Error.TrackAsync(ex.ToString());
                await AppHealth.Current.Exception.TrackAsync(ex);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async void OnCheckUpdates()
        {
            try
            {
                await GenContext.ToolBox.Repo.CheckForUpdatesAsync();
            }
            catch (Exception ex)
            {
                Status = new StatusViewModel(StatusType.Information, StringRes.ErrorSyncRefresh, true);

                await AppHealth.Current.Error.TrackAsync(ex.ToString());
                await AppHealth.Current.Exception.TrackAsync(ex);
            }
            finally
            {
                IsLoading = false;
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
                Status = new StatusViewModel(StatusType.Information, StringRes.ErrorSync, true);

                await AppHealth.Current.Error.TrackAsync(ex.ToString());
                await AppHealth.Current.Exception.TrackAsync(ex);
            }
            finally
            {
                IsLoading = false;
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
        private void SyncSyncStatusChanged(object sender, SyncStatusEventArgs status)
        {
            Status = new StatusViewModel(StatusType.Information, GetStatusText(status.Status), true);

            if (status.Status == SyncStatus.Updated)
            {
                TemplatesVersion = GenContext.ToolBox.Repo.TemplatesVersion;
                CleanStatus();

                _templatesAvailable = true;
                OnTemplatesAvailable();
                NextCommand.OnCanExecuteChanged();
            }
            if (status.Status == SyncStatus.OverVersion)
            {
                _mainView.Dispatcher.Invoke(() =>
                {
                    Status = new StatusViewModel(StatusType.Warning, StringRes.StatusOverVersionContent);
                });
            }

            if (status.Status == SyncStatus.OverVersionNoContent)
            {
                _mainView.Dispatcher.Invoke(() =>
                {
                    Status = new StatusViewModel(StatusType.Error, StringRes.StatusOverVersionNoContent);
                    _templatesAvailable = false;
                    NextCommand.OnCanExecuteChanged();
                });
            }

            if (status.Status == SyncStatus.UnderVersion)
            {
                _mainView.Dispatcher.Invoke(() =>
                {
                    Status = new StatusViewModel(StatusType.Error, StringRes.StatusLowerVersionContent);
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
