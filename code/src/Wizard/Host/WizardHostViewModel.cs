using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Extensions;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.Wizard.Resources;
using Microsoft.Templates.Wizard.Steps;
using Microsoft.Templates.Wizard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Templates.Wizard.Host
{
    public class WizardHostViewModel : Observable
    {
        private WizardContext _context;

        public WizardHost Host { get; }
        public WizardSteps Steps { get; }

        public WizardHostViewModel(WizardHost host, WizardSteps steps, TemplatesRepository templatesRepository, GenShell shell)
        {
            //TODO: VERIFY NOT NULL
            Host = host;
            Steps = steps;
            _context = new WizardContext(templatesRepository, shell);

            _context.PropertyChanged += _context_PropertyChanged;

            NextButtonText = WizardHostResources.NextButton;
        }

        public WizardHostViewModel()
        {
        }

        public async Task IniatializeAsync()
        {
            _context.TemplatesRepository.Sync += (sender, status) =>
            {
                Status = GetStatusText(status);

                if (status == SyncStatus.Updated)
                {
                    _context.CanGoForward = true;

                    var step = Steps.First();
                    Navigate(step);
                }
            };

            try
            {
                await _context.TemplatesRepository.SynchronizeAsync();
                Status = string.Empty;
            }
            catch (Exception ex)
            {
                Status = StringRes.ErrorSync;

                await AppHealth.Current.Error.TrackAsync(ex.ToString());
                await AppHealth.Current.Exception.TrackAsync(ex);
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

            OnPropertyChanged("PreviousCommand");
            OnPropertyChanged("NextCommand");

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
                OnPropertyChanged("NextCommand");
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
                currentStep.CleanState();

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
