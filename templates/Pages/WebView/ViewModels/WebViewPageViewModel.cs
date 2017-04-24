using System;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Param_ItemNamespace.ViewModels
{
    public class WebViewPageViewModel : System.ComponentModel.INotifyPropertyChanged
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

        private ICommand _navCompleted;
        public ICommand NavCompletedCommand
        {
            get
            {
                if (_navCompleted == null)
                {
                    _navCompleted = new RelayCommand<WebViewNavigationCompletedEventArgs>(NavCompleted);
                }

                return _navCompleted;
            }
        }

        private void NavCompleted(WebViewNavigationCompletedEventArgs e)
        {
            IsLoading = false;
        }

        private ICommand _navFailed;
        public ICommand NavFailedCommand
        {
            get
            {
                if (_navFailed == null)
                {
                    _navFailed = new RelayCommand<WebViewNavigationFailedEventArgs>(NavFailed);
                }

                return _navFailed;
            }
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
                    _retryCommand = new RelayCommand(Retry);
                }

                return _retryCommand;
            }
        }

        private void Retry()
        {
            IsShowingFailedMessage = false;
            IsLoading = true;

            _webView?.Refresh();
        }

        private WebView _webView;

        public WebViewPageViewModel()
        {
            IsLoading = true;
            Source = new Uri(defaultUrl);
        }

        public void Initialize(WebView webView)
        {
            _webView = webView;
        }
    }
}
