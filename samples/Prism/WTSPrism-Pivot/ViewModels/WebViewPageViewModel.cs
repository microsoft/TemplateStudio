using System;
using System.Windows.Input;

using Prism.Commands;
using Prism.Windows.Mvvm;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WTSPrism.Services;

namespace WTSPrism.ViewModels
{
    public class WebViewPageViewModel : ViewModelBase
    {
        // TODO WTS: Set your hyperlink default here
        private const string defaultUrl = "https://developer.microsoft.com/en-us/windows/apps";

        private Uri _source;
        public Uri Source
        {
            get { return _source; }
            set { SetProperty(ref _source, value); }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (value) IsShowingFailedMessage = false;
                SetProperty(ref _isLoading, value);
                IsLoadingVisibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private Visibility _isLoadingVisibility;
        public Visibility IsLoadingVisibility
        {
            get { return _isLoadingVisibility; }
            set
            {
                SetProperty(ref _isLoadingVisibility, value);
            }
        }

        private bool _isShowingFailedMessage;
        public bool IsShowingFailedMessage
        {
            get { return _isShowingFailedMessage; }
            set
            {
                if (value) IsLoading = false;
                SetProperty(ref _isShowingFailedMessage, value);
                FailedMesageVisibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private Visibility _failedMesageVisibility;
        public Visibility FailedMesageVisibility
        {
            get { return _failedMesageVisibility; }
            set
            {
                SetProperty(ref _failedMesageVisibility, value);
            }
        }

        private IWebViewService webViewService;

        public IWebViewService WebViewService
        {
            get { return webViewService; }
            set
            {
                if (webViewService != null)
                {
                    webViewService.NavigationComplete -= WebViewService_NavigationComplete;
                    webViewService.NavigationFailed -= WebViewService_NavigationFailed;
                }
                webViewService = value;
                webViewService.NavigationComplete += WebViewService_NavigationComplete;
                webViewService.NavigationFailed += WebViewService_NavigationFailed;
            }
        }

        private void WebViewService_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {
            NavFailed(e);
        }

        private void WebViewService_NavigationComplete(object sender, WebViewNavigationCompletedEventArgs e)
        {
            NavCompleted(e);
        }

        private void NavCompleted(WebViewNavigationCompletedEventArgs e)
        {
            IsLoading = false;
            RaisePropertyChanged(nameof(BrowserBackCommand));
            RaisePropertyChanged(nameof(BrowserForwardCommand));
        }

        private void NavFailed(WebViewNavigationFailedEventArgs e)
        {
            // Use `e.WebErrorStatus` to vary the displayed message based on the error reason
            IsShowingFailedMessage = true;
        }

        private ICommand _retryCommand;
        public ICommand RetryCommand
        {
            get
            {
                if (_retryCommand == null)
                {
                    _retryCommand = new DelegateCommand(Retry);
                }

                return _retryCommand;
            }
        }

        private void Retry()
        {
            IsShowingFailedMessage = false;
            IsLoading = true;

            webViewService?.Refresh();
        }

        private ICommand _browserBackCommand;
        public ICommand BrowserBackCommand
        {
            get
            {
                if (_browserBackCommand == null)
                {
                    _browserBackCommand = new DelegateCommand(() => webViewService?.GoBack(), () => webViewService?.CanGoBack ?? false);
                }

                return _browserBackCommand;
            }
        }

        private ICommand _browserForwardCommand;
        public ICommand BrowserForwardCommand
        {
            get
            {
                if (_browserForwardCommand == null)
                {
                    _browserForwardCommand = new DelegateCommand(() => webViewService?.GoForward(), () => webViewService?.CanGoForward ?? false);
                }

                return _browserForwardCommand;
            }
        }

        private ICommand _refreshCommand;
        public ICommand RefreshCommand
        {
            get
            {
                if (_refreshCommand == null)
                {
                    _refreshCommand = new DelegateCommand(() => webViewService?.Refresh());
                }

                return _refreshCommand;
            }
        }

        private ICommand _openInBrowserCommand;
        public ICommand OpenInBrowserCommand
        {
            get
            {
                if (_openInBrowserCommand == null)
                {
                    _openInBrowserCommand = new DelegateCommand(async () => await Windows.System.Launcher.LaunchUriAsync(Source));
                }

                return _openInBrowserCommand;
            }
        }

        public WebViewPageViewModel()
        {
            IsLoading = true;
            Source = new Uri(defaultUrl);
        }
    }
}
