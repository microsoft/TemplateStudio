using Microsoft.Templates.Wizard.Host;
using System.Windows;

namespace Microsoft.Templates.Wizard.Steps.FrameworkTypeStep
{
    /// <summary>
    /// Interaction logic for PagesStepPage.xaml
    /// </summary>
    public partial class FrameworkTypeStepPage : StepPage
    {
        //TODO: SAVE LATEST SELECTED BY THE USER

        public FrameworkTypeStepViewModel ViewModel { get; }

        public FrameworkTypeStepPage(WizardContext context) : base(context)
        {
            ViewModel = new FrameworkTypeStepViewModel(context);

            DataContext = ViewModel;
            Loaded += OnPageLoaded;
            InitializeComponent();
        }

        private async void OnPageLoaded(object sender, RoutedEventArgs e) => await ViewModel.LoadDataAsync();
        public override string PageTitle => FrameworkTypeStepResources.PageTitle;
    }
}
