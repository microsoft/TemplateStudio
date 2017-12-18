using System;
using Windows.UI.Xaml.Controls;

namespace WTSPrism.Services
{
    public interface IWebViewService
    {
        bool CanGoBack { get; }
        bool CanGoForward { get; }

        void GoBack();
        void GoForward();
        void Refresh();

        event EventHandler<WebViewNavigationCompletedEventArgs> NavigationComplete;

        event EventHandler<WebViewNavigationFailedEventArgs> NavigationFailed;
    }
}
