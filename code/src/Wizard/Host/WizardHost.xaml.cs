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
        public WizardState Result { get; set; }

        public WizardHost(WizardSteps wizardSteps)
        {
            ViewModel = new WizardHostViewModel(this, wizardSteps);

            DataContext = ViewModel;

            Loaded += async (sender, e) =>
            {
                await ViewModel.IniatializeAsync();
            };

            InitializeComponent();
        }
    }
}
