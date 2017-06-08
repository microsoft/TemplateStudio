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

using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Controls;
using Microsoft.Templates.UI.Extensions;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.Services;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Templates.UI.ViewModels.Common
{
    public abstract class BaseMainViewModel : Observable
    {
        private Window _mainView;
        private bool _templatesReady;
        protected bool _canGoBack;
        protected bool _canGoForward;
        protected bool _hasValidationErrors;
        protected bool _templatesAvailable;
        protected OverlayBox _overlayBox;

        protected StatusViewModel _status = StatusControl.EmptyStatus;
        public StatusViewModel Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
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

        protected bool _newUpdateAvailable;
        public bool NewUpdateAvailable
        {
            get => _newUpdateAvailable;
            set => SetProperty(ref _newUpdateAvailable, value);
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

        protected Visibility _noContentVisibility = Visibility.Collapsed;
        public Visibility NoContentVisibility
        {
            get => _noContentVisibility;
            set => SetProperty(ref _noContentVisibility, value);
        }        

        protected bool _showFinishButton;
        public bool ShowFinishButton
        {
            get => _showFinishButton;
            set => SetProperty(ref _showFinishButton, value);
        }

        #region Commands
        private RelayCommand _showOverlayMenuCommand;
        protected RelayCommand _cancelCommand;
        protected RelayCommand _goBackCommand;
        protected RelayCommand _nextCommand;
        protected RelayCommand _finishCommand;

        public RelayCommand CancelCommand => _cancelCommand ?? (_cancelCommand = new RelayCommand(OnCancel));
        public RelayCommand BackCommand => _goBackCommand ?? (_goBackCommand = new RelayCommand(OnGoBack, () => _canGoBack));
        public RelayCommand NextCommand => _nextCommand ?? (_nextCommand = new RelayCommand(OnNext, () => _templatesAvailable && !_hasValidationErrors && _canGoForward));
        public RelayCommand ShowOverlayMenuCommand => _showOverlayMenuCommand ?? (_showOverlayMenuCommand = new RelayCommand(OnShowOverlayMenu));        
        public RelayCommand FinishCommand => _finishCommand ?? (_finishCommand = new RelayCommand(OnFinish, CanFinish));
        #endregion        

        public BaseMainViewModel(Window mainView)
        {
            _mainView = mainView;
        }

        protected abstract void OnCancel();
        protected virtual void OnNext()
        {
            _canGoBack = true;
            BackCommand.OnCanExecuteChanged();
            ShowFinishButton = true;
        }
        protected abstract void OnTemplatesAvailable();
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
        protected virtual bool CanFinish()
        {
            if (_hasValidationErrors || !_templatesReady)
            {
                return false;
            }
            CleanStatus();
            return true;
        }
        protected virtual void OnFinish()
        {
            _mainView.DialogResult = true;
            _mainView.Close();
        }        
        protected async Task BaseInitializeAsync(OverlayBox overlayBox)
        {
            _overlayBox = overlayBox;
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
                case SyncStatus.Adquiring:
                    return StringRes.StatusAdquiring;
                case SyncStatus.Adquired:
                    return StringRes.StatusAdquired;
                case SyncStatus.Preparing:
                    return StringRes.StatusPreparing;
                case SyncStatus.Prepared:
                    return StringRes.StatusPrepared;
                default:
                    return string.Empty;
            }
        }        
        private void SyncSyncStatusChanged(object sender, SyncStatus status)
        {
            Status = new StatusViewModel(StatusType.Information, GetStatusText(status), true);

            if (status == SyncStatus.Updated)
            {
                TemplatesVersion = GenContext.ToolBox.Repo.TemplatesVersion;
                CleanStatus();

                _templatesAvailable = true;
                OnTemplatesAvailable();
                NextCommand.OnCanExecuteChanged();
            }
            if (status == SyncStatus.OverVersion)
            {
                _mainView.Dispatcher.Invoke(() =>
                {
                    Status = new StatusViewModel(StatusType.Warning, StringRes.StatusOverVersionContent);
                });
            }

            if (status == SyncStatus.OverVersionNoContent)
            {
                _mainView.Dispatcher.Invoke(() =>
                {
                    Status = new StatusViewModel(StatusType.Error, StringRes.StatusOverVersionNoContent);
                    _templatesAvailable = false;
                    NextCommand.OnCanExecuteChanged();
                });
            }

            if (status == SyncStatus.UnderVersion)
            {
                _mainView.Dispatcher.Invoke(() =>
                {
                    Status = new StatusViewModel(StatusType.Error, StringRes.StatusLowerVersionContent);
                    _templatesAvailable = false;
                    NextCommand.OnCanExecuteChanged();
                });
            }
        }        
        private async void OnShowOverlayMenu()
        {
            if (_overlayBox.Opacity == 0)
            {
                Panel.SetZIndex(_overlayBox, 2);
                await _overlayBox.FadeInAsync();                
            }
            else
            {
                Panel.SetZIndex(_overlayBox, 0);
                await _overlayBox.FadeOutAsync();                
            }
        }
    }
}
