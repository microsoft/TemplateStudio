using System;
using Microsoft.UI.Xaml.Controls;
using Windows.Web;

namespace Param_RootNamespace.Contracts.Services
{
    public interface IWebViewService
    {
        event EventHandler<WebErrorStatus> NavigationCompleted;

        bool CanGoBack { get; }

        bool CanGoForward { get; }

        void Initialize(WebView2 webView);

        void UnregisterEvents();

        void GoBack();

        void GoForward();

        void Reload();
    }
}
