//{[{
using Windows.System;
using Windows.UI.Xaml.Input;
using Param_RootNamespace.ViewModels;
//}]}
namespace Param_RootNamespace.Services
{
    internal class ActivationService
    {
//^^
//{[{
        public static NavigationServiceEx NavigationService => ViewModelLocator.Current.NavigationService;

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
//}]}
                }
        }
//{[{

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
//}]}
    }
}
