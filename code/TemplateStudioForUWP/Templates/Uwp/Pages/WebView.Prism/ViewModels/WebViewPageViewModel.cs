using System;
using System.Windows.Input;
using Prism.Commands;
using Prism.Windows.Mvvm;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Param_RootNamespace.Services;

namespace Param_RootNamespace.ViewModels
{
    public class WebViewPageViewModel : ViewModelBase
    {
        // TODO WTS: Set the URI of the page to show by default
        private const string DefaultUrl = "https://docs.microsoft.com/windows/apps/";

        public WebViewPageViewModel()
        {
            IsLoading = true;
            Source = new Uri(DefaultUrl);

            BrowserBackCommand = new DelegateCommand(() => _webViewService?.GoBack(), () => _webViewService?.CanGoBack ?? false);
            BrowserForwardCommand = new DelegateCommand(() => _webViewService?.GoForward(), () => _webViewService?.CanGoForward ?? false);
            RefreshCommand = new DelegateCommand(() => _webViewService?.Refresh());
            RetryCommand = new DelegateCommand(Retry);
            OpenInBrowserCommand = new DelegateCommand(async () => await Windows.System.Launcher.LaunchUriAsync(Source));

            // Note that the WebViewService is set from within the view because it needs a reference to the WebView control
        }

        private Uri _source;

        public Uri Source
        {
            get { return _source; }
            set { Param_Setter(ref _source, value); }
        }

        private bool _isLoading;

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

                Param_Setter(ref _isLoading, value);
                IsLoadingVisibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private Visibility _isLoadingVisibility;

        public Visibility IsLoadingVisibility
        {
            get
            {
                return _isLoadingVisibility;
            }

            set
            {
                Param_Setter(ref _isLoadingVisibility, value);
            }
        }

        private bool _isShowingFailedMessage;

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

                Param_Setter(ref _isShowingFailedMessage, value);
                FailedMesageVisibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private Visibility _failedMessageVisibility;

        public Visibility FailedMesageVisibility
        {
            get
            {
                return _failedMessageVisibility;
            }

            set
            {
                Param_Setter(ref _failedMessageVisibility, value);
            }
        }

        private IWebViewService _webViewService;

        public IWebViewService WebViewService
        {
            get
            {
                return _webViewService;
            }

            // the WebViewService is set from within the view (instead of IoC) because it needs a reference to the control
            set
            {
                if (_webViewService != null)
                {
                    _webViewService.NavigationComplete -= WebViewService_NavigationComplete;
                    _webViewService.NavigationFailed -= WebViewService_NavigationFailed;
                }

                _webViewService = value;
                _webViewService.NavigationComplete += WebViewService_NavigationComplete;
                _webViewService.NavigationFailed += WebViewService_NavigationFailed;
            }
        }

        public ICommand BrowserBackCommand { get; }

        public ICommand BrowserForwardCommand { get; }

        public ICommand RefreshCommand { get; }

        public ICommand RetryCommand { get; }

        public ICommand OpenInBrowserCommand { get; }

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

        private void Retry()
        {
            IsShowingFailedMessage = false;
            IsLoading = true;

            _webViewService?.Refresh();
        }
    }
}
