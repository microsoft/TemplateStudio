using Microsoft.UI.Xaml.Controls;
using WinUIDesktopApp.ViewModels;

namespace WinUIDesktopApp.Views
{
    // TODO WTS: Change the icons and titles for all NavigationViewItems in ShellWindow.xaml.
    public sealed partial class ShellPage : Page
    {
        public ShellViewModel ViewModel { get; }

        public ShellPage(ShellViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
            ViewModel.NavigationService.Frame = shellFrame;
            ViewModel.NavigationViewService.Initialize(navigationView);
        }
    }
}
