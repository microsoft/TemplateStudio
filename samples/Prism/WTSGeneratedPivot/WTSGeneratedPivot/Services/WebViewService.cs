using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml.Controls;

namespace WTSGeneratedPivot.Services
{
    public class WebViewService : IWebViewService
    {
        private WebView webView;

        public WebViewService(WebView webViewInstance)
        {
            webView = webViewInstance;
            webView.NavigationCompleted += WebView_NavigationCompleted;
            webView.NavigationFailed += WebView_NavigationFailed;
        }

        public void Detatch()
        {
            if (webView != null)
            {
                webView.NavigationCompleted -= WebView_NavigationCompleted;
                webView.NavigationFailed += WebView_NavigationFailed;
            }
        }

        private void WebView_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {
            NavigationFailed?.Invoke(sender, e);
        }

        private void WebView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs e)
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
