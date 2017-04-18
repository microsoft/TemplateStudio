using Microsoft.Templates.UI.ViewModels;
using System.IO;
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
                stepFrame.Navigate(new ProjectSetupView());
            };

            Unloaded += (sender, e) =>
            {
                ViewModel.UnsuscribeEventHandlers();
            };
            InitializeComponent();
        }
    }
}
