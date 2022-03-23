using Param_RootNamespace.Services;
using Param_RootNamespace.Views;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Param_RootNamespace.Helpers
{
    public static class MenuNavigationHelper
    {
        private static object _lastParamUsed;
        private static SplitView _splitView;
        private static Frame _rightFrame;

        public static void Initialize(SplitView splitView, Frame rightFrame)
        {
            _splitView = splitView;
            _rightFrame = rightFrame;
        }

        public static void UpdateView(Type pageType, object parameters = null, NavigationTransitionInfo infoOverride = null)
        {
             NavigationService.Navigate(pageType, parameters, infoOverride, true);
        }

        public static void Navigate(Type pageType, object parameter = null, NavigationTransitionInfo infoOverride = null)
        {
            NavigationService.Navigate(pageType, parameter, infoOverride);
        }

        public static void OpenInRightPane(Type pageType, object parameter = null, NavigationTransitionInfo infoOverride = null)
        {
            // Don't open the same page multiple times
            if (_rightFrame.Content?.GetType() != pageType || (parameter != null && !parameter.Equals(_lastParamUsed)))
            {
                var navigationResult = _rightFrame.Navigate(pageType, parameter, infoOverride);
                if (navigationResult)
                {
                    _lastParamUsed = parameter;
                }
            }

            _splitView.IsPaneOpen = true;
        }

        public static async Task OpenInNewWindow(Type pageType)
        {
            await WindowManagerService.Current.TryShowAsStandaloneAsync(pageType.Name, pageType);
        }

        public static async Task OpenInDialog(Type pageType, object parameter = null, NavigationTransitionInfo infoOverride = null)
        {
            var dialog = ShellContentDialog.Create(pageType, parameter, infoOverride);
            await dialog.ShowAsync();
        }
    }
}
