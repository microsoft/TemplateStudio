// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Threading;
using Microsoft.Templates.UI.V2Controls;
using Microsoft.Templates.UI.V2Extensions;

namespace Microsoft.Templates.UI.V2ViewModels.Common
{
    public abstract class BaseMainViewModel : Observable
    {
        private int _step;
        private bool _canGoBack = false;
        private bool _canGoForward = true;

        private RelayCommand _cancelCommand;
        private RelayCommand _goBackCommand;
        private RelayCommand _goForwardCommand;
        private RelayCommand _finishCommand;

        protected string Language { get; private set; }

        public int Step
        {
            get => _step;
            set
            {
                var goForward = value > _step;
                SetProperty(ref _step, value);
                UpdateStep();
            }
        }

        public RelayCommand CancelCommand => _cancelCommand ?? (_cancelCommand = new RelayCommand(OnCancel));

        public RelayCommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new RelayCommand(() => Step--, () => _canGoBack && !WizardStatus.IsBusy));

        public RelayCommand GoForwardCommand => _goForwardCommand ?? (_goForwardCommand = new RelayCommand(() => Step++, () => _canGoForward && !WizardStatus.IsBusy));

        public RelayCommand FinishCommand => _finishCommand ?? (_finishCommand = new RelayCommand(OnFinish, () => !WizardStatus.IsBusy));

        public WizardStatus WizardStatus { get; } = new WizardStatus();

        protected abstract void UpdateStep();

        protected abstract void OnCancel();

        protected abstract void OnFinish();

        protected abstract Task OnTemplatesAvailableAsync();

        public BaseMainViewModel()
        {
            WizardStatus.IsBusyChanged += IsBusyChanged;
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
            System.Diagnostics.Debug.WriteLine(args.Status);
            await NotificationsControl.Instance.AddNotificationAsync(args.GetNotification());
            if (args.Status == SyncStatus.Updated)
            {
                await OnTemplatesAvailableAsync();
            }
            else if (args.Status == SyncStatus.Acquired)
            {
                // TODO: Turn on the light that indicates that there are templates updates.
            }
        }
    }
}
