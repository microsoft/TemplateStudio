using Microsoft.Templates.UI.ViewModels;
using System.Windows.Controls;

namespace Microsoft.Templates.UI.Views
{
    /// <summary>
    /// Interaction logic for ProjectSetupView.xaml
    /// </summary>
    public partial class ProjectSetupView : Page
    {
        public MainViewModel ViewModel { get; }
        public ProjectSetupView()
        {
            ViewModel = MainViewModel.Current;
            DataContext = ViewModel;

            Loaded += async (sender, e) =>
            {
                await ViewModel.ProjectSetup.IniatializeAsync();
            };

            InitializeComponent();
        }
    }
}
