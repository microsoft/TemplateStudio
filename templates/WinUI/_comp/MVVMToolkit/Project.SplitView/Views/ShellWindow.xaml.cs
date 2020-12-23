using Microsoft.UI.Xaml;
using Param_RootNamespace.Contracts.Views;
using Param_RootNamespace.Helpers;
using Param_RootNamespace.ViewModels;

namespace Param_RootNamespace.Views
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
