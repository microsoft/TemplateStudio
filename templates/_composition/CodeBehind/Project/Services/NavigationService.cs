using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Param_RootNamespace.Services
{
    public static class NavigationService
    {
        
        public static event NavigatedEventHandler Navigated;
        public static event NavigationFailedEventHandler NavigationFailed;

        private static bool _frameEventsRegistrated;
        private static Frame _frame;

        public static Frame Frame
        {
            get
            {
                if (_frame == null)
                {
                    _frame = Window.Current.Content as Frame;
                    RegisterFrameEvents();
                }

                return _frame;
            }

            set
            {
                UnregisterFrameEvents();
                _frame = value;
                RegisterFrameEvents();
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

        public static bool Navigate<T>(object parameter = null, NavigationTransitionInfo infoOverride = null)
            where T : Page
            => Navigate(typeof(T), parameter, infoOverride);
        
        private static void RegisterFrameEvents()
        {
            _frame.Navigated += _frame_Navigated;
            _frame.NavigationFailed += _frame_NavigationFailed;
            _frameEventsRegistrated = true;
        }

        private static void UnregisterFrameEvents()
        {
            if (_frameEventsRegistrated)
            {
                _frame.Navigated -= _frame_Navigated;
                _frame.NavigationFailed -= _frame_NavigationFailed;
                _frameEventsRegistrated = false;
            }            
        }

        private static void _frame_NavigationFailed(object sender, NavigationFailedEventArgs e) => NavigationFailed?.Invoke(sender, e);
        private static void _frame_Navigated(object sender, NavigationEventArgs e) => Navigated?.Invoke(sender, e);
    }
}
