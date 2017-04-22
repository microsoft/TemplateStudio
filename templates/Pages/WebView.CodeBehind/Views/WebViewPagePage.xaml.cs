using System;
using Windows.UI.Xaml.Controls;

namespace Param_ItemNamespace.Views
{
    public sealed partial class WebViewPagePage : Page, System.ComponentModel.INotifyPropertyChanged
    {
        // TODO UWPTemplates: Set your hyperlink default here
        private const string defaultUrl = "https://developer.microsoft.com/en-us/windows/apps";

        private Uri _source;
        public Uri Source
        {
            get { return _source; }
            set { Set(ref _source, value); }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (value) IsShowingFailedMessage = false;
                Set(ref _isLoading, value);
                IsLoadingVisibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private Visibility _isLoadingVisibility;
        public Visibility IsLoadingVisibility
        {
            get { return _isLoadingVisibility; }
            set
            {
                Set(ref _isLoadingVisibility, value);
            }
        }

        private bool _isShowingFailedMessage;
        public bool IsShowingFailedMessage
        {
            get { return _isShowingFailedMessage; }
            set
            {
                if (value) IsLoading = false;
                Set(ref _isShowingFailedMessage, value);
                FailedMesageVisibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private Visibility _failedMesageVisibility;
        public Visibility FailedMesageVisibility
        {
            get { return _failedMesageVisibility; }
            set
            {
                Set(ref _failedMesageVisibility, value);
            }
        }

        private void OnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            IsLoading = false;
        }

        private void OnNavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {
            // Use `e.WebErrorStatus` to vary the displayed message based on the error reason
            IsShowingFailedMessage = true;
        }

        private void OnRetry(object sender, RoutedEventArgs e)
        {
            IsShowingFailedMessage = false;
            IsLoading = true;

            webView.Refresh();
        }

        public WebViewPagePage()
        {
            Source = new Uri(defaultUrl);
            InitializeComponent();
            IsLoading = true;
        }
    }
}
