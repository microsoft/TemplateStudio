//{[{
using Windows.System;
using Windows.UI.Xaml.Input;
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
                    }
//}]}
                }
            }

//^^
//{[{
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
