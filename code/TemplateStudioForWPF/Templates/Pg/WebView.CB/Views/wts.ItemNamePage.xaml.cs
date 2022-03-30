using System.Windows;
using System.Windows.Controls;
using Microsoft.Web.WebView2.Core;
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Contracts.Views;

namespace Param_RootNamespace.Views
{
    public partial class wts.ItemNamePage : Page, INotifyPropertyChanged, INavigationAware
    {
        // TODO: Set the URI of the page to show by default
        private const string DefaultUrl = "https://docs.microsoft.com/windows/apps/";

        private readonly ISystemService _systemService;
        private string _source;
        private bool _isLoading = true;
        private bool _isShowingFailedMessage;
        private bool _canBrowserBack;
        private bool _canBrowserForward;
        private Visibility _isLoadingVisibility = Visibility.Visible;
        private Visibility _failedMesageVisibility = Visibility.Collapsed;

        public string Source
        {
            get { return _source; }
            set { Set(ref _source, value); }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                Set(ref _isLoading, value);
                IsLoadingVisibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public bool IsShowingFailedMessage
        {
            get => _isShowingFailedMessage;
            set
            {
                Set(ref _isShowingFailedMessage, value);
                FailedMesageVisibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public bool CanBrowserBack
        {
            get { return _canBrowserBack; }
            set { Set(ref _canBrowserBack, value); }
        }

        public bool CanBrowserForward
        {
            get { return _canBrowserForward; }
            set { Set(ref _canBrowserForward, value); }
        }

        public Visibility IsLoadingVisibility
        {
            get { return _isLoadingVisibility; }
            set { Set(ref _isLoadingVisibility, value); }
        }

        public Visibility FailedMesageVisibility
        {
            get { return _failedMesageVisibility; }
            set { Set(ref _failedMesageVisibility, value); }
        }

        public wts.ItemNamePage(ISystemService systemService)
        {
            _systemService = systemService;
            Source = DefaultUrl;
            InitializeComponent();
        }

        public void OnNavigatedTo(object parameter)
        {
        }

        public void OnNavigatedFrom()
        {
        }

        private void OnBrowserBack(object sender, RoutedEventArgs e)
            => webView.GoBack();

        private void OnBrowserForward(object sender, RoutedEventArgs e)
            => webView.GoForward();

        private void OnRefresh(object sender, RoutedEventArgs e)
        {
            IsShowingFailedMessage = false;
            IsLoading = true;
            webView.Reload();
        }

        private void OnOpenInBrowser(object sender, RoutedEventArgs e)
            => _systemService.OpenInWebBrowser(Source);

        private void OnNavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            IsLoading = false;
            if (e != null && !e.IsSuccess)
            {
                // Use `e.WebErrorStatus` to vary the displayed message based on the error reason
                IsShowingFailedMessage = true;
            }

            CanBrowserBack = webView.CanGoBack;
            CanBrowserForward = webView.CanGoForward;
        }
    }
}
