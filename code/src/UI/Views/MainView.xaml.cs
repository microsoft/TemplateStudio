using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.ViewModels;
using System.Windows;

namespace Microsoft.Templates.UI.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainViewModel ViewModel { get; }
        public WizardState Result { get; set; }

        public MainView()
        {

            ViewModel = new MainViewModel(this);

            DataContext = ViewModel;

            Loaded += async (sender, e) =>
            {
                await ViewModel.IniatializeAsync();
                NavigationService.Initialize(stepFrame, new ProjectSetupView());
            };

            Unloaded += (sender, e) =>
            {
                ViewModel.UnsuscribeEventHandlers();
            };
            InitializeComponent();            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ProjectTemplates.Pages.Add(new TemplateInfoViewModel
            {
                Name = "Page",
                TemplateName = "Blank Page",
                Author = "Javito",
                IsEnabled = false
            });
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ViewModel.ProjectTemplates.Features.Add(new TemplateInfoViewModel
            {
                Name = "Feature",
                TemplateName = "Suspend and Resume",
                Author = "Javito",
                IsEnabled = true,
                HasDefaultName = true
            });
        }
    }
}
