using uct.SplitViewProject.Services;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace uct.SplitViewProject.Views
{
    public sealed partial class ShellPage : Page
    {
        public ShellPage()
        {
            DataContext = ViewModel;
            this.InitializeComponent();

            ViewModel.Initialize(frame);
        }
    }
}
