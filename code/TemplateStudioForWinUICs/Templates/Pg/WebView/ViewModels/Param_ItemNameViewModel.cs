using System.Windows.Input;
using Microsoft.Web.WebView2.Core;
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Contracts.ViewModels;

namespace Param_RootNamespace.ViewModels;

// TODO: Review best practices and distribution guidelines for WebView2.
// https://docs.microsoft.com/microsoft-edge/webview2/get-started/winui
// https://docs.microsoft.com/microsoft-edge/webview2/concepts/developer-guide
// https://docs.microsoft.com/microsoft-edge/webview2/concepts/distribution
public partial class Param_ItemNameViewModel : System.ComponentModel.INotifyPropertyChanged, INavigationAware
{
    // TODO: Set the default URL to display.
    private Uri _source = new("https://docs.microsoft.com/windows/apps/");
    private bool _isLoading = true;

    public IWebViewService WebViewService
    {
        get;
    }

    public Uri Source
    {
        get => _source;
        set => SetProperty(ref _source, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    [ObservableProperty]
    private bool hasFailures;

    public Param_ItemNameViewModel(IWebViewService webViewService)
    {
        WebViewService = webViewService;
    }
    [RelayCommand]
    private async Task OpenInBrowser()
    {
        if (WebViewService.Source != null)
        {
            await Windows.System.Launcher.LaunchUriAsync(WebViewService.Source);
        }
    }
    [RelayCommand]
    private void Reload()
    {
        WebViewService.Reload();
    }
    [RelayCommand]
    private void BrowserForward()
    {
        if (WebViewService.CanGoForward)
        {
            WebViewService.GoForward();
        }
    }
    [RelayCommand]
    private void BrowserBack()
    {
        if (WebViewService.CanGoBack)
        {
            WebViewService.GoBack();
        }
    }
    [RelayCommand]
    private void OnRetry()
    {
        hasFailures = false;
        IsLoading = true;
        WebViewService?.Reload();
    }
    public void OnNavigatedTo(object parameter)
    {
        WebViewService.NavigationCompleted += OnNavigationCompleted;
    }

    public void OnNavigatedFrom()
    {
        WebViewService.UnregisterEvents();
        WebViewService.NavigationCompleted -= OnNavigationCompleted;
    }

    private void OnNavigationCompleted(object? sender, CoreWebView2WebErrorStatus webErrorStatus)
    {
        IsLoading = false;
        OnPropertyChanged(nameof(BrowserBackCommand));
        OnPropertyChanged(nameof(BrowserForwardCommand));
        if (webErrorStatus != default)
        {
            hasFailures = true;
        }
    }
}
