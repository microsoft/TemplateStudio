//{[{
using Param_RootNamespace.Helpers;
//}]}

namespace Param_RootNamespace.Services
{
    public class NavigationServiceEx
    {
//^^
//{[{
        public event EventHandler<bool> OnCurrentPageCanGoBackChanged;

//}]}
        public event NavigationFailedEventHandler NavigationFailed;
        private object _lastParamUsed;
//{[{
        private bool _canCurrentPageGoBack;
//}]}
        public bool GoBack()
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
        private void RegisterFrameEvents()
        {
            if (_frame != null)
            {
                _frame.Navigated += Frame_Navigated;
//{[{
                _frame.Navigating += Frame_Navigating;
//}]}
            }
        }

        private void UnregisterFrameEvents()
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
        private void Frame_Navigated(object sender, NavigationEventArgs e) => Navigated?.Invoke(sender, e);
//}--}
//^^
//{[{
        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            if (Frame.Content is FrameworkElement element && element.DataContext is IBackNavigationHandler backNavigationHandler)
            {
                backNavigationHandler.OnPageCanGoBackChanged += OnPageCanGoBackChanged;
            }

            Navigated?.Invoke(sender, e);
        }

        private void Frame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (Frame.Content is FrameworkElement element && element.DataContext is IBackNavigationHandler backNavigationHandler)
            {
                backNavigationHandler.OnPageCanGoBackChanged -= OnPageCanGoBackChanged;
                _canCurrentPageGoBack = false;
            }
        }

        private void OnPageCanGoBackChanged(object sender, bool canCurrentPageGoBack)
        {
            _canCurrentPageGoBack = canCurrentPageGoBack;
            OnCurrentPageCanGoBackChanged?.Invoke(sender, canCurrentPageGoBack);
        }
//}]}
    }
}