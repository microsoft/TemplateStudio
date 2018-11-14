//{[{
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
//}]}
namespace Param_RootNamespace.Services
{
    internal class ActivationService
    {
//^^
//{[{
        public static readonly KeyboardAccelerator AltLeftKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu);

        public static readonly KeyboardAccelerator BackKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.GoBack);
//}]}

        public ActivationService(App app, Type defaultNavItem, Lazy<UIElement> shell = null)
        {            
        }

        public async Task ActivateAsync(object activationArgs)
        {
                if (Window.Current.Content == null)
                {
                    Window.Current.Content = _shell?.Value ?? new Frame();
//{[{
                    NavigationService.NavigationFailed += (sender, e) =>
                    {
                        throw e.Exception;
                    };
                    NavigationService.Navigated += Frame_Navigated;
                    if (SystemNavigationManager.GetForCurrentView() != null)
                    {
                        SystemNavigationManager.GetForCurrentView().BackRequested += ActivationService_BackRequested;
                    }
//}]}
                }
        }
//{[{

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = NavigationService.CanGoBack ?
                AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
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
            var result = NavigationService.GoBack();
            args.Handled = result;
        }

        private void ActivationService_BackRequested(object sender, BackRequestedEventArgs e)
        {
            var result = NavigationService.GoBack();
            e.Handled = result;
        }
//}]}
    }
}
