using Microsoft.UI.Xaml;

using WinUI3App.Contracts.Views;
using WinUI3App.ViewModels;

namespace WinUI3App.Views
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
