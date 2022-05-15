using System;
using System.Windows.Input;
using Microsoft.Web.WebView2.Core;
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Contracts.ViewModels;

namespace Param_RootNamespace.ViewModels
{
    // TODO: Review best practices and distribution guidelines for WebView2.
    // https://docs.microsoft.com/microsoft-edge/webview2/get-started/winui
    // https://docs.microsoft.com/microsoft-edge/webview2/concepts/developer-guide
    // https://docs.microsoft.com/microsoft-edge/webview2/concepts/distribution
    public class Param_ItemNameViewModel : System.ComponentModel.INotifyPropertyChanged, INavigationAware
    {
        // TODO: Set the default URL to display.
        private const string DefaultUrl = "https://docs.microsoft.com/windows/apps/";
        private Uri _source;
        private bool _isLoading = true;
        private bool _hasFailures;
        private ICommand _browserBackCommand;
        private ICommand _browserForwardCommand;
        private ICommand _openInBrowserCommand;
        private ICommand _reloadCommand;
        private ICommand _retryCommand;

        public IWebViewService WebViewService { get; }

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

        public bool HasFailures
        {
            get => _hasFailures;
            set => SetProperty(ref _hasFailures, value);
        }

        public ICommand BrowserBackCommand => _browserBackCommand ??=
            new RelayCommand(() => WebViewService?.GoBack(), () => WebViewService?.CanGoBack ?? false);

        public ICommand BrowserForwardCommand => _browserForwardCommand ??=
            new RelayCommand(() => WebViewService?.GoForward(), () => WebViewService?.CanGoForward ?? false);

        public ICommand ReloadCommand => _reloadCommand ??=
            new RelayCommand(() => WebViewService?.Reload());

        public ICommand RetryCommand => _retryCommand ??=
            new RelayCommand(OnRetry);

        public ICommand OpenInBrowserCommand => _openInBrowserCommand ??=
            new RelayCommand(async() => await Windows.System.Launcher.LaunchUriAsync(Source));

        public Param_ItemNameViewModel(IWebViewService webViewService)
        {
            WebViewService = webViewService;
        }

        public void OnNavigatedTo(object parameter)
        {
            WebViewService.NavigationCompleted += OnNavigationCompleted;
            Source = new Uri(DefaultUrl);
        }

        public void OnNavigatedFrom()
        {
            WebViewService.UnregisterEvents();
            WebViewService.NavigationCompleted -= OnNavigationCompleted;
        }

        private void OnNavigationCompleted(object sender, CoreWebView2WebErrorStatus webErrorStatus)
        {
            IsLoading = false;
            OnPropertyChanged(nameof(BrowserBackCommand));
            OnPropertyChanged(nameof(BrowserForwardCommand));
            if (webErrorStatus != default)
            {
                HasFailures = true;
            }
        }

        private void OnRetry()
        {
            HasFailures = false;
            IsLoading = true;
            WebViewService?.Reload();
        }
    }
}
