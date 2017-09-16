using System;
using Windows.UI.Xaml.Controls;

namespace WTSPrism.Services
{
    public class WebViewService : IWebViewService
    {
        private WebView webView;

        public WebViewService(WebView webView)
        {
            this.webView = webView;
            this.webView.NavigationCompleted += webView_NavigationCompleted;
            this.webView.NavigationFailed += webView_NavigationFailed;
        }

        public void Detatch()
        {
            if (webView != null)
            {
                webView.NavigationCompleted -= webView_NavigationCompleted;
                webView.NavigationFailed += webView_NavigationFailed;
            }
        }

        private void webView_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {
            NavigationFailed?.Invoke(sender, e);
        }

        private void webView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs e)
        {
            NavigationComplete?.Invoke(sender, e);
        }

        public void Refresh()
        {
            webView?.Refresh();
        }

        public void GoForward()
        {
            webView?.GoForward();
        }

        public void GoBack()
        {
            webView?.GoBack();
        }

        public bool CanGoForward => webView?.CanGoForward == true;

        public bool CanGoBack => webView?.CanGoBack == true;

        public event EventHandler<WebViewNavigationCompletedEventArgs> NavigationComplete;

        public event EventHandler<WebViewNavigationFailedEventArgs> NavigationFailed;
    }
}
