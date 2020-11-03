using Microsoft.UI.Xaml;

using WinUIDesktopApp.Contracts.Views;
using WinUIDesktopApp.ViewModels;

namespace WinUIDesktopApp.Views
{
    public sealed partial class ShellWindow : Window, IShellWindow
    {
        public ShellViewModel ViewModel { get; }

        public ShellWindow(ShellViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
            ViewModel.Initialize(shellFrame, navigationView);
        }
    }
}
