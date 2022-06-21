using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;

using Param_RootNamespace.Contracts.Services;

namespace Param_RootNamespace.Services;

public class MockWebViewService : IWebViewService
{
    private WebView2 _webView;

    public bool CanGoBack => _webView.CanGoBack;

    public bool CanGoForward => _webView.CanGoForward;

    public event EventHandler<CoreWebView2WebErrorStatus> NavigationCompleted;

    public MockWebViewService()
    {
    }

    public void Initialize(WebView2 webView)
    {
    }

    public void GoBack()
    {
    }
    public void GoForward()
    {
    }

    public void Reload()
    {
    }

    public void UnregisterEvents()
    {
    }
}
