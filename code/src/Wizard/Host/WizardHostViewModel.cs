using Microsoft.Templates.Core;
using Microsoft.Templates.Wizard.Dialog;
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
    public class WizardHostViewModel : ObservableBase
    {
        private WizardContext _context;

        public WizardHost Host { get; }
        public WizardSteps Steps { get; }

        public WizardHostViewModel(WizardHost host, WizardSteps steps, TemplatesRepository templatesRepository, GenShell shell)
        {
            //TODO: VERIFY NOT NULL
            Host = host;
            Steps = steps;
            _context = new WizardContext(templatesRepository, shell) { CanGoForward = true };
            _context.PropertyChanged += _context_PropertyChanged;

            NextButtonText = WizardHostResources.NextButton;
        }

        public void Iniatialize()
        {
            try
            {
                _context.TemplatesRepository.Sync();
            }
            catch (RepositorySynchronizationException ex)
            {
                ErrorMessageDialog.Show("Ops! It is not possible to update available templates. Please ensure you have internet connection and that you are running the latest version of the extension.", "Updating available templates.", ex.ToString(), MessageBoxImage.Warning);
            }

            var step = Steps.First();
            Navigate(step);
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
