using System;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace WinUIDesktopApp.ViewModels
{
    public class WebViewViewModel : ObservableRecipient
    {
        // TODO WTS: Set the URI of the page to show by default
        private const string DefaultUrl = "https://docs.microsoft.com/windows/apps/";

        private WebView2 _webView;
        private Uri _source;
        private bool _isLoading;
        private bool _isShowingFailedMessage;
        private Visibility _isLoadingVisibility;
        private Visibility _failedMesageVisibility;
        private ICommand _browserBackCommand;
        private ICommand _browserForwardCommand;
        private ICommand _openInBrowserCommand;
        private ICommand _reloadCommand;
        private ICommand _retryCommand;
        private ICommand _navigationCompletedCommand;

        public Uri Source
        {
            get { return _source; }
            set { SetProperty(ref _source, value); }
        }

        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                if (value)
                {
                    IsShowingFailedMessage = false;
                }

                SetProperty(ref _isLoading, value);
                IsLoadingVisibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public Visibility IsLoadingVisibility
        {
            get { return _isLoadingVisibility; }
            set { SetProperty(ref _isLoadingVisibility, value); }
        }

        public bool IsShowingFailedMessage
        {
            get
            {
                return _isShowingFailedMessage;
            }

            set
            {
                if (value)
                {
                    IsLoading = false;
                }

                SetProperty(ref _isShowingFailedMessage, value);
                FailedMesageVisibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public Visibility FailedMesageVisibility
        {
            get { return _failedMesageVisibility; }
            set { SetProperty(ref _failedMesageVisibility, value); }
        }

        public ICommand BrowserBackCommand => _browserBackCommand ?? (_browserBackCommand = new RelayCommand(
            () => _webView?.GoBack(), () => _webView?.CanGoBack ?? false));

        public ICommand BrowserForwardCommand => _browserForwardCommand ?? (_browserForwardCommand = new RelayCommand(
            () => _webView?.GoForward(), () => _webView?.CanGoForward ?? false));

        public ICommand ReloadCommand => _reloadCommand ?? (_reloadCommand = new RelayCommand(
            () => _webView?.Reload()));

        public ICommand RetryCommand => _retryCommand ?? (_retryCommand = new RelayCommand(OnRetry));

        public ICommand OpenInBrowserCommand => _openInBrowserCommand ?? (_openInBrowserCommand = new RelayCommand(async
            () => await Windows.System.Launcher.LaunchUriAsync(Source)));

        public ICommand NavigationCompletedCommand => _navigationCompletedCommand ?? (_navigationCompletedCommand = new RelayCommand<WebView2NavigationCompletedEventArgs>(OnNavigationCompleted));

        public WebViewViewModel()
        {
            IsLoading = true;
            Source = new Uri(DefaultUrl);
        }

        public void Initialize(WebView2 webView)
        {
            _webView = webView;
        }

        private void OnNavigationCompleted(WebView2NavigationCompletedEventArgs args)
        {
            IsLoading = false;
            OnPropertyChanged(nameof(BrowserBackCommand));
            OnPropertyChanged(nameof(BrowserForwardCommand));
            if (args.WebErrorStatus != default)
            {
                // Use `e.WebErrorStatus` to vary the displayed message based on the error reason
                IsShowingFailedMessage = true;
            }
        }

        private void OnRetry()
        {
            IsShowingFailedMessage = false;
            IsLoading = true;

            _webView?.Reload();
        }
    }
}
