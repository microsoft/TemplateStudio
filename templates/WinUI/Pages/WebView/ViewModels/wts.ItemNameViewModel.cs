using System;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Windows.Web;
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Contracts.ViewModels;

namespace Param_RootNamespace.ViewModels
{
    public class wts.ItemNameViewModel : System.ComponentModel.INotifyPropertyChanged, INavigationAware
    {
        // TODO WTS: Set the URI of the page to show by default
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
            get { return _source; }
            set { SetProperty(ref _source, value); }
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

        public ICommand BrowserBackCommand => _browserBackCommand ?? (_browserBackCommand = new RelayCommand(
            () => WebViewService?.GoBack(), () => WebViewService?.CanGoBack ?? false));

        public ICommand BrowserForwardCommand => _browserForwardCommand ?? (_browserForwardCommand = new RelayCommand(
            () => WebViewService?.GoForward(), () => WebViewService?.CanGoForward ?? false));

        public ICommand ReloadCommand => _reloadCommand ?? (_reloadCommand = new RelayCommand(
            () => WebViewService?.Reload()));

        public ICommand RetryCommand => _retryCommand ?? (_retryCommand = new RelayCommand(OnRetry));

        public ICommand OpenInBrowserCommand => _openInBrowserCommand ?? (_openInBrowserCommand = new RelayCommand(async
            () => await Windows.System.Launcher.LaunchUriAsync(Source)));

        public wts.ItemNameViewModel(IWebViewService webViewService)
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

        private void OnNavigationCompleted(object sender, WebErrorStatus webErrorStatus)
        {
            IsLoading = false;
            OnPropertyChanged(nameof(BrowserBackCommand));
            OnPropertyChanged(nameof(BrowserForwardCommand));
            if (webErrorStatus != default)
            {
                // Use `webErrorStatus` to vary the displayed message based on the error reason
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
