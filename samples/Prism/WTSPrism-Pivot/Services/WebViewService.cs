using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace WTSPrism.Services
{
    public class WebViewService : IWebViewService
    {
        private WebView _webView;

        public WebViewService(WebView webView)
        {
            _webView = webView;
            _webView.NavigationCompleted += _webView_NavigationCompleted;
            _webView.NavigationFailed += _webView_NavigationFailed;
        }

        public void Detatch()
        {
            if (_webView != null)
            {
                _webView.NavigationCompleted -= _webView_NavigationCompleted;
                _webView.NavigationFailed += _webView_NavigationFailed;
            }
        }

        private void _webView_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {
            NavigationFailed?.Invoke(sender, e);
        }

        private void _webView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs e)
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

        public bool CanGoForward
        {
            get { return _webView?.CanGoForward == true; }
        }

        public bool CanGoBack
        {
            get { return _webView?.CanGoBack == true; }
        }

        public event EventHandler<WebViewNavigationCompletedEventArgs> NavigationComplete;

        public event EventHandler<WebViewNavigationFailedEventArgs> NavigationFailed;
    }
}
