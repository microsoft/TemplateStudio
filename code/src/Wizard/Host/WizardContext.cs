using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.Wizard.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Microsoft.Templates.Wizard.Host
{
    public class WizardState
    {
        public string ProjectType { get; set; }
        public string Framework { get; set; }
        public List<(string name, string templateName)> Pages { get; } = new List<(string name, string templateName)>();
        public List<(string name, string templateName)> DevFeatures { get; } = new List<(string name, string templateName)>();
        public List<(string name, string templateName)> ConsumerFeatures { get; } = new List<(string name, string templateName)>();
    }

    public class WizardContext : Observable
    {
        public WizardState State { get; } = new WizardState();

        private bool _canGoForward;
        public bool CanGoForward
        {
            get => _canGoForward;
            set => SetProperty(ref _canGoForward, value);
        }

        public bool ResetSelection()
        {
            var resetSelectionResult = MessageBox.Show(Host.WizardHostResources.ResetSelection, Host.WizardHostResources.ResetSelectionTitle, MessageBoxButton.YesNo, MessageBoxImage.Warning);
            return resetSelectionResult == MessageBoxResult.Yes;
        }
    }
}
