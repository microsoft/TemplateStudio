using Microsoft.Templates.Wizard.Host;
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

namespace Microsoft.Templates.Wizard.Steps.DevFeatures.NewDevFeature
{
    /// <summary>
    /// Interaction logic for NewDevFeatureDialog.xaml
    /// </summary>
    public partial class NewDevFeatureDialog : Window
    {
        public NewDevFeatureViewModel ViewModel { get; }
        public (string name, string templateName) Result { get; set; }

        public NewDevFeatureDialog(WizardContext context, IEnumerable<string> selectedNames)
        {
            ViewModel = new NewDevFeatureViewModel(context, this, selectedNames);

            DataContext = ViewModel;
            Loaded += NewDevFeatureDialog_Loaded;
            InitializeComponent();
        }

        private async void NewDevFeatureDialog_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.InitializeAsync();
        }
    }
}
