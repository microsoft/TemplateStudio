using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Param_RootNamespace.Services
{
    public class WebViewService : IWebViewService
    {
        private WebView _webView;

        public WebViewService(WebView webViewInstance)
        {
            _webView = webViewInstance;
            _webView.NavigationCompleted += WebView_NavigationCompleted;
            _webView.NavigationFailed += WebView_NavigationFailed;
        }

        public void Detatch()
        {
            if (_webView != null)
            {
                _webView.NavigationCompleted -= WebView_NavigationCompleted;
                _webView.NavigationFailed += WebView_NavigationFailed;
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
            _webView?.Refresh();
        }

        public void GoForward()
        {
            _webView?.GoForward();
        }

        public void GoBack()
        {
            _webView?.GoBack();
        }

        public bool CanGoForward => _webView?.CanGoForward == true;

        public bool CanGoBack => _webView?.CanGoBack == true;

        public event EventHandler<WebViewNavigationCompletedEventArgs> NavigationComplete;

        public event EventHandler<WebViewNavigationFailedEventArgs> NavigationFailed;
    }
}
