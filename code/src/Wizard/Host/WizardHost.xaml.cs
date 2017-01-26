using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Wizard.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Microsoft.Templates.Wizard.Host
{
    /// <summary>
    /// Interaction logic for WizardHost.xaml
    /// </summary>
    public partial class WizardHost : Window
    {
        public WizardHostViewModel ViewModel { get; }
        public IEnumerable<GenInfo> Result { get; set; }

        public WizardHost(WizardSteps wizardSteps, TemplatesRepository templatesRepository, GenShell shell)
        {
            ViewModel = new WizardHostViewModel(this, wizardSteps, templatesRepository, shell);

            DataContext = ViewModel;
            Loaded += WizardHost_Loaded;

            InitializeComponent();
        }

        private void WizardHost_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Iniatialize();
        }
    }
}
