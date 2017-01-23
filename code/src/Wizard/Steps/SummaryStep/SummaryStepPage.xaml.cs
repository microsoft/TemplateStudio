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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Microsoft.Templates.Wizard.Steps.SummaryStep
{
    /// <summary>
    /// Interaction logic for SummaryStepPage.xaml
    /// </summary>
    public partial class SummaryStepPage : StepPage
    {
        public SummaryStepViewModel ViewModel { get; }

        public SummaryStepPage(WizardContext context) : base(context)
        {
            ViewModel = new SummaryStepViewModel(context);

            DataContext = ViewModel;
            Loaded += SummaryStepPage_Loaded;

            InitializeComponent();
        }

        private async void SummaryStepPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadDataAsync();
        }

        public override string PageTitle => SummaryStepResources.PageTitle;
    }
}
