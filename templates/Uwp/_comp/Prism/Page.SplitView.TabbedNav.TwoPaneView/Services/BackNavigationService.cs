using System;
using Param_RootNamespace.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Param_RootNamespace.Services
{
    public class BackNavigationService : IBackNavigationService
    {
        public event EventHandler<bool> OnCurrentPageCanGoBackChanged;

        public BackNavigationService()
        {
        }

        public void Initialize(Frame frame)
        {
            frame.Navigated += OnFrameNavigated;
            frame.Navigating += OnFrameNavigating;
        }

        private void OnFrameNavigated(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            if (sender is Frame frame && frame.Content is FrameworkElement element && element.DataContext is IBackNavigationHandler backNavigationHandler)
            {
                backNavigationHandler.OnPageCanGoBackChanged += OnPageCanGoBackChanged;
            }
        }

        private void OnFrameNavigating(object sender, Windows.UI.Xaml.Navigation.NavigatingCancelEventArgs e)
        {
            if (sender is Frame frame && frame.Content is FrameworkElement element && element.DataContext is IBackNavigationHandler backNavigationHandler)
            {
                backNavigationHandler.OnPageCanGoBackChanged -= OnPageCanGoBackChanged;
            }
        }

        private void OnPageCanGoBackChanged(object sender, bool canCurrentPageGoBack)
        {
            OnCurrentPageCanGoBackChanged?.Invoke(sender, canCurrentPageGoBack);
        }
    }
}
