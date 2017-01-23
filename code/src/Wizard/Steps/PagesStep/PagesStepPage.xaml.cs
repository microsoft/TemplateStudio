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

namespace Microsoft.Templates.Wizard.Steps.PagesStep
{
    /// <summary>
    /// Interaction logic for PagesStepPage.xaml
    /// </summary>
    public partial class PagesStepPage : StepPage
    {
        //TODO: SAVE LATEST SELECTED BY THE USER

        public PagesStepViewModel ViewModel { get; }

        public PagesStepPage(WizardContext context) : base(context)
        {
            ViewModel = new PagesStepViewModel(context);

            DataContext = ViewModel;
            Loaded += PagesStepPage_Loaded;
            InitializeComponent();
        }

        private async void PagesStepPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadDataAsync();
        }

        public override string PageTitle => PagesStepResources.PageTitle;
    }
}
