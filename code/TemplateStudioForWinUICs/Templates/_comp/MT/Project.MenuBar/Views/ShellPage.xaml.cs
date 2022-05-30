using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Helpers;
using Param_RootNamespace.ViewModels;
using Windows.System;

namespace Param_RootNamespace.Views
{
    public sealed partial class ShellPage : Page
    {
        public ShellViewModel ViewModel
        {
            get;
        }

        public ShellPage(ShellViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();

            ViewModel.NavigationService.Frame = NavigationFrame;

            // TODO: Set the title bar icon by updating /Assets/WindowIcon.png.
            // A custom title bar is required for full window theme and Mica support.
            // https://docs.microsoft.com/windows/apps/develop/title-bar?tabs=winui3#full-customization
            App.MainWindow.ExtendsContentIntoTitleBar = true;
            App.MainWindow.SetTitleBar(AppTitleBar);
            App.MainWindow.Activated += MainWindow_Activated;
            AppTitleBarText.Text = "AppDisplayName".GetLocalized();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            TitleBarHelper.UpdateTitleBar(RequestedTheme);

            KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu));
            KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.GoBack));
        }

        private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
        {
            var resource = args.WindowActivationState == WindowActivationState.Deactivated ? "WindowCaptionForegroundDisabled" : "WindowCaptionForeground";

            AppTitleBarText.Foreground = (SolidColorBrush)App.Current.Resources[resource];
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
        }

        private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
        {
            var keyboardAccelerator = new KeyboardAccelerator() { Key = key };

            if (modifiers.HasValue)
            {
                keyboardAccelerator.Modifiers = modifiers.Value;
            }

            keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;

            return keyboardAccelerator;
        }

        private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            var navigationService = App.GetService<INavigationService>();

            var result = navigationService.GoBack();

            args.Handled = result;
        }
    }
}
