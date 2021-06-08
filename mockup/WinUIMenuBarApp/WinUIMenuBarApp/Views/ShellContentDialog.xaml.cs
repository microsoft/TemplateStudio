using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUIMenuBarApp.Contracts.Views;
using WinUIMenuBarApp.ViewModels;

namespace WinUIMenuBarApp.Views
{
    public sealed partial class ShellContentDialog : ContentDialog, IShellContentDialog
    {
        public ShellContentDialogViewModel ViewModel { get; }

        public ShellContentDialog(ShellContentDialogViewModel viewModel)
        {
            ViewModel = viewModel;
            XamlRoot = App.MainWindow.Content.XamlRoot;
            InitializeComponent();
            RequestedTheme = (App.MainWindow.Content as FrameworkElement).RequestedTheme;
        }

        public Frame GetDialogFrame()
            => dialogFrame;
    }
}