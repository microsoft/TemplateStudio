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
            _context = new WizardContext(templatesRepository, shell);
            _context.PropertyChanged += _context_PropertyChanged;

            NextButtonText = WizardHostResources.NextButton;
        }

        public void Iniatialize()
        {
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

        private void Navigate(Type pageType)
        {
            //TODO: CACHE INSTANCES
            var stepPage = Activator.CreateInstance(pageType, new object[] { _context }) as StepPage;

            StepTitle = stepPage.PageTitle;

            Host.StepsFrame.Navigate(stepPage);

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

        private void _context_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_context.CanGoForward))
            {
                OnPropertyChanged("NextCommand");
            }
        }

        private void GoForward()
        {
            _context.NotifySave();

            if (Steps.CanGoForward())
            {
                var step = Steps.GoForward();
                Navigate(step);
            }
            else
            {
                Host.DialogResult = true;
                Host.Result = _context.SelectedTemplates
                                            .SelectMany(t => t.Value);
                Host.Close();
            }
        }

        private void GoBack()
        {
            if (Steps.CanGoBack())
            {
                //TODO: REMOVE STATE FROM PREVIOUS PAGE
                var step = Steps.GoBack();
                Navigate(step);
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
