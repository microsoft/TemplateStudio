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
        private const string DefaultUrl = "https://developer.microsoft.com/en-us/windows/apps";

        public WebViewPageViewModel()
        {
            IsLoading = true;
            Source = new Uri(DefaultUrl);

            BrowserBackCommand = new DelegateCommand(() => webViewService?.GoBack(), () => webViewService?.CanGoBack ?? false);
            BrowserForwardCommand = new DelegateCommand(() => webViewService?.GoForward(), () => webViewService?.CanGoForward ?? false);
            RefreshCommand = new DelegateCommand(() => webViewService?.Refresh());
            RetryCommand = new DelegateCommand(Retry);
            OpenInBrowserCommand = new DelegateCommand(async () => await Windows.System.Launcher.LaunchUriAsync(Source));

            // Note that the WebViewService is set from within the view because it needs a reference to the WebView control
        }

        private Uri source;
        public Uri Source
        {
            get { return source; }
            set { SetProperty(ref source, value); }
        }

        private bool isLoading;
        public bool IsLoading
        {
            get { return isLoading; }
            set
            {
                if (value) IsShowingFailedMessage = false;
                SetProperty(ref isLoading, value);
                IsLoadingVisibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private Visibility isLoadingVisibility;
        public Visibility IsLoadingVisibility
        {
            get { return isLoadingVisibility; }
            set
            {
                SetProperty(ref isLoadingVisibility, value);
            }
        }

        private bool isShowingFailedMessage;
        public bool IsShowingFailedMessage
        {
            get { return isShowingFailedMessage; }
            set
            {
                if (value) IsLoading = false;
                SetProperty(ref isShowingFailedMessage, value);
                FailedMesageVisibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private Visibility failedMesageVisibility;
        public Visibility FailedMesageVisibility
        {
            get { return failedMesageVisibility; }
            set
            {
                SetProperty(ref failedMesageVisibility, value);
            }
        }

        private IWebViewService webViewService;
        public IWebViewService WebViewService
        {
            get { return webViewService; }
            // the WebViewService is set from within the view (instead of IoC) because it needs a reference to the control
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

            webViewService?.Refresh();
        }
    }
}
