using Microsoft.UI.Xaml;

using WinUIDesktopApp.Contracts.Views;
using WinUIDesktopApp.Helpers;
using WinUIDesktopApp.ViewModels;

namespace WinUIDesktopApp.Views
{
    public sealed partial class ShellWindow : Window, IShellWindow
    {
        public ShellViewModel ViewModel { get; }

        public ShellWindow(ShellViewModel viewModel)
        {
            Title = "AppDisplayName".GetLocalized();
            ViewModel = viewModel;
            InitializeComponent();
            ViewModel.NavigationService.Frame = shellFrame;
            ViewModel.NavigationViewService.Initialize(navigationView);
        }
    }
}
