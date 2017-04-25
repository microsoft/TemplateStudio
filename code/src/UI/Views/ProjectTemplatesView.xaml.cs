using Microsoft.Templates.UI.ViewModels;
using System.Windows.Controls;

namespace Microsoft.Templates.UI.Views
{
    /// <summary>
    /// Interaction logic for ProjectTemplatesView.xaml
    /// </summary>
    public partial class ProjectTemplatesView : Page
    {
        public ProjectTemplatesViewModel ViewModel { get; }
        public ProjectTemplatesView()
        {
            ViewModel = MainViewModel.Current.ProjectTemplates;
            DataContext = ViewModel;

            Loaded += async (sender, e) =>
            {
                await ViewModel.IniatializeAsync();
            };

            InitializeComponent();            
        }        
    }
}
