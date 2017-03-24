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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Extensions;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.Wizard.Resources;
using Microsoft.Templates.Wizard.Steps;
using Microsoft.Templates.Wizard.ViewModels;

namespace Microsoft.Templates.Wizard.Host
{
    public class WizardHostViewModel : Observable
    {
        private WizardContext _context;

        public WizardHost Host { get; }
        public WizardSteps Steps { get; }

        public WizardHostViewModel(WizardHost host, WizardSteps steps)
        {
            //TODO: VERIFY NOT NULL
            Host = host;
            Steps = steps;
            _context = new WizardContext()
            {
                Host = host
            };

            _context.PropertyChanged += _context_PropertyChanged;

            NextButtonText = WizardHostResources.NextButton;
        }

        public WizardHostViewModel()
        {
        }

        public async Task IniatializeAsync()
        {
            GenContext.ToolBox.Repo.Sync.SyncStatusChanged += Sync_SyncStatusChanged;

            try
            {
                WizardVersion = GetWizardVersion();

                await GenContext.ToolBox.Repo.SynchronizeAsync();
                Status = string.Empty;

                TemplatesVersion = GenContext.ToolBox.Repo.GetVersion();
            }
            catch (Exception ex)
            {
                Status = StringRes.ErrorSync;

                await AppHealth.Current.Error.TrackAsync(ex.ToString());
                await AppHealth.Current.Exception.TrackAsync(ex);
            }

        }

        private void Sync_SyncStatusChanged(object sender, SyncStatus status)
        {

            Status = GetStatusText(status);

            if (status == SyncStatus.Updated)
            {
                TemplatesVersion = GenContext.ToolBox.Repo.GetVersion();

                _context.CanGoForward = true;
                
                var step = Steps.First();
                Navigate(step);
            }


            if (status == SyncStatus.OverVersion)
            {
                Host.Dispatcher.Invoke(() => 
                {
                    MessageBox.Show(Host, StringRes.StatusOverVersionContent, StringRes.StatusOverVersionTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                });

                //TODO: Review message and behavior.
            }

            if (status == SyncStatus.UnderVersion)
            {
                Host.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(Host, StringRes.StatusLowerVersionContent, StringRes.StatusLowerVersionContent, MessageBoxButton.OK, MessageBoxImage.Error);
                    //TODO: Review message and behavior.
                });
            }
        }

        public void UnsuscribeEventHandlers()
        {
            GenContext.ToolBox.Repo.Sync.SyncStatusChanged -= Sync_SyncStatusChanged;
        }

        private string GetWizardVersion()
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var versionInfo = FileVersionInfo.GetVersionInfo(assemblyLocation);

            return versionInfo.FileVersion;
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
                default:
                    return string.Empty;
            }
        }

        private string _status;
        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        private string _wizardVersion;
        public string WizardVersion
        {
            get { return _wizardVersion; }
            set { SetProperty(ref _wizardVersion, value); }
        }

        private string _templatesVersion;
        public string TemplatesVersion
        {
            get { return _templatesVersion; }
            set { SetProperty(ref _templatesVersion, value); }
        }

        private string _stepTitle;
        public string StepTitle
        {
            get { return _stepTitle; }
            set { SetProperty(ref _stepTitle, value); }
        }

        private string _nextButtonText;
        public string NextButtonText
        {
            get { return _nextButtonText; }
            set { SetProperty(ref _nextButtonText, value); }
        }

        public RelayCommand CancelCommand => new RelayCommand(Host.Close);
        public RelayCommand PreviousCommand => new RelayCommand(GoBack, Steps.CanGoBack);
        public RelayCommand NextCommand => new RelayCommand(GoForward, CanGoForward);

        private void Navigate(Type stepType)
        {
            //TODO: CACHE INSTANCES??
            var nextStep = Activator.CreateInstance(stepType, new object[] { _context }) as StepViewModel;

            StepTitle = nextStep.PageTitle;

            Host.StepsFrame.Navigate(nextStep.GetPage());

            OnPropertyChanged(nameof(PreviousCommand));
            OnPropertyChanged(nameof(NextCommand));

            if (Steps.CanGoForward())
            {
                NextButtonText = WizardHostResources.NextButton;
            }
            else
            {
                NextButtonText = WizardHostResources.FinishButton;
            }
        }

        private StepViewModel GetCurrentStep()
        {
            var currentPage = Host.StepsFrame.Content as Page;
            return currentPage?.DataContext as StepViewModel;
        }

        private void _context_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_context.CanGoForward))
            {
                OnPropertyChanged(nameof(NextCommand));
            }
        }

        private void GoForward()
        {
            if (Steps.CanGoForward())
            {
                var currentStep = GetCurrentStep();
                currentStep?.SaveState();

                var nextStep = Steps.GoForward();
                Navigate(nextStep);
            }
            else
            {
                Host.DialogResult = true;
                Host.Result = _context.State;
                Host.Close();
            }
        }

        private void GoBack()
        {
            if (Steps.CanGoBack())
            {
                var currentStep = GetCurrentStep();

                var previous = Steps.GoBack();
                Navigate(previous);
            }
        }

        private void Cancel()
        {
            Host.DialogResult = false;
            Host.Result = null;

            Host.Close();
        }

        private bool CanGoForward() => _context.CanGoForward;
    }
}
