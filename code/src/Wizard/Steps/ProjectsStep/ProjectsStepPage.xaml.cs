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

using Microsoft.Templates.Wizard.Host;

namespace Microsoft.Templates.Wizard.Steps.ProjectsStep
{
    /// <summary>
    /// Interaction logic for ProjectsStepPage.xaml
    /// </summary>
    public partial class ProjectsStepPage : StepPage
    {
        //TODO: SAVE LATEST SELECTED BY THE USER

        public ProjectsStepViewModel ViewModel { get; }

        public ProjectsStepPage(WizardContext context) : base(context)
        {
            ViewModel = new ProjectsStepViewModel(context);

            DataContext = ViewModel;
            Loaded += ProjectsStepPage_Loaded;
            InitializeComponent();
        }

        private async void ProjectsStepPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadDataAsync();
        }

        public override string PageTitle => ProjectsStepResources.PageTitle;
    }
}
