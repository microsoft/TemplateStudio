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
        public static KeyboardAccelerator AltLeftKeyboardAccelerator { get; private set; }

        public static KeyboardAccelerator BackKeyboardAccelerator { get; private set; }
//}]}

        public ActivationService(WinRTContainer container, Type defaultNavItem, Lazy<UIElement> shell = null)
        {
//^^
//{[{
            AltLeftKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu);
            BackKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.GoBack);
//}]}
        }

        public async Task ActivateAsync(object activationArgs)
        {
            if (IsInteractive(activationArgs))
            {
                if (Window.Current.Content == null)
                {
                    if (_shell?.Value == null)
                    {
                    }
                    else
                    {
                    }
//{[{

                    if (NavigationService != null)
                    {
                        NavigationService.NavigationFailed += (sender, e) =>
                        {
                            throw e.Exception;
                        };

                        NavigationService.Navigated += OnFrameNavigated;
                    }
//}]}
                }
            }

//^^
//{[{
        private void OnFrameNavigated(object sender, NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = NavigationService.CanGoBack ?
                AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        private KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
        {
            var keyboardAccelerator = new KeyboardAccelerator() { Key = key };
            if (modifiers.HasValue)
            {
                keyboardAccelerator.Modifiers = modifiers.Value;
            }

            keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;
            return keyboardAccelerator;
        }

        private void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
                args.Handled = true;
            }
        }
//}]}
    }
}
