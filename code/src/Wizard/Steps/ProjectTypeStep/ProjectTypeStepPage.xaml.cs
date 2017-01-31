using Microsoft.Templates.Wizard.Host;
using System.Windows;

namespace Microsoft.Templates.Wizard.Steps.ProjectTypeStep
{
    /// <summary>
    /// Interaction logic for PagesStepPage.xaml
    /// </summary>
    public partial class ProjectTypeStepPage : StepPage
    {
        //TODO: SAVE LATEST SELECTED BY THE USER

        public ProjectTypeStepViewModel ViewModel { get; }

        public ProjectTypeStepPage(WizardContext context) : base(context)
        {
            ViewModel = new ProjectTypeStepViewModel(context);

            DataContext = ViewModel;
            Loaded += OnPageLoaded;
            InitializeComponent();
        }

        private async void OnPageLoaded(object sender, RoutedEventArgs e) => await ViewModel.LoadDataAsync();
        public override string PageTitle => ProjectTypeStepResources.PageTitle;
    }
}
