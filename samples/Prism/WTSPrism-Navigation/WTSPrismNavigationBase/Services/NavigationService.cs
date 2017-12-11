using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace WTSPrismNavigationBase.Services
{
    public static class NavigationService
    {
        private static Frame _frame;
        public static Frame Frame
        {
            get
            {
                if (_frame == null)
                {
                    _frame = Window.Current.Content as Frame;
                }

                return _frame;
            }
            set
            {
                _frame = value;
            }
        }

        public static bool CanGoBack => Frame.CanGoBack;
        public static bool CanGoForward => Frame.CanGoForward;

        public static void GoBack() => Frame.GoBack();
        public static void GoForward() => Frame.GoForward();

        public static bool Navigate(Type pageType, object parameter = null, NavigationTransitionInfo infoOverride = null)
        {
            // Don't open the same page multiple times
            if (Frame.Content?.GetType() != pageType)
            {
                return Frame.Navigate(pageType, parameter, infoOverride);
            }
            else
            {
                return false;
            }
        }

        public static bool Navigate<T>(object parameter = null, NavigationTransitionInfo infoOverride = null) where T : Page => Navigate(typeof(T), parameter, infoOverride);
    }
}
