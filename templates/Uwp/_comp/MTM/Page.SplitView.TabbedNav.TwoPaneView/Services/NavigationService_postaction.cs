//{[{
using Param_RootNamespace.Helpers;
//}]}

namespace Param_RootNamespace.Services
{
    public static class NavigationService
    {
//^^
//{[{
        public static event EventHandler<bool> OnCurrentPageCanGoBackChanged;

//}]}
        public static event NavigationFailedEventHandler NavigationFailed;
        private static object _lastParamUsed;
//{[{
        private static bool _canCurrentPageGoBack;
//}]}
        public static bool GoBack()
        {
//{[{
            if (_canCurrentPageGoBack)
            {
                if (Frame.Content is FrameworkElement element && element.DataContext is IBackNavigationHandler navigationHandler)
                {
                    navigationHandler.GoBack();
                    return true;
                }
            }

//}]}
        }
        private static void RegisterFrameEvents()
        {
            if (_frame != null)
            {
                _frame.Navigated += Frame_Navigated;
//{[{
                _frame.Navigating += Frame_Navigating;
//}]}
            }
        }

        private static void UnregisterFrameEvents()
        {
            if (_frame != null)
            {
                _frame.Navigated -= Frame_Navigated;
//{[{
                _frame.Navigating -= Frame_Navigating;
//}]}
            }
        }
//{--{
        private static void Frame_Navigated(object sender, NavigationEventArgs e) => Navigated?.Invoke(sender, e);
//}--}
//^^
//{[{
        private static void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            if (Frame.Content is FrameworkElement element && element.DataContext is IBackNavigationHandler backNavigationHandler)
            {
                backNavigationHandler.OnPageCanGoBackChanged += OnPageCanGoBackChanged;
            }

            Navigated?.Invoke(sender, e);
        }

        private static void Frame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (Frame.Content is FrameworkElement element && element.DataContext is IBackNavigationHandler backNavigationHandler)
            {
                backNavigationHandler.OnPageCanGoBackChanged -= OnPageCanGoBackChanged;
                _canCurrentPageGoBack = false;
            }
        }

        private static void OnPageCanGoBackChanged(object sender, bool canCurrentPageGoBack)
        {
            _canCurrentPageGoBack = canCurrentPageGoBack;
            OnCurrentPageCanGoBackChanged?.Invoke(sender, canCurrentPageGoBack);
        }
//}]}
    }
}