using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Microsoft.Toolkit.Wpf.UI.Controls;

namespace Param_RootNamespace.ViewModels
{
    public class wts.ItemNameViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        // TODO WTS: Set the URI of the page to show by default
        private const string DefaultUrl = "https://docs.microsoft.com/windows/apps/";

        private string _source;
        private bool _isLoading = true;
        private bool _isShowingFailedMessage = false;
        private Visibility _isLoadingVisibility = Visibility.Visible;
        private Visibility _failedMesageVisibility = Visibility.Collapsed;
        private ICommand _refreshCommand;
        private System.Windows.Input.ICommand _browserBackCommand;
        private System.Windows.Input.ICommand _browserForwardCommand;
        private ICommand _openInBrowserCommand;
        private WebView _webView;

        public string Source
        {
            get { return _source; }
            set { Param_Setter(ref _source, value); }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                Param_Setter(ref _isLoading, value);
                IsLoadingVisibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public bool IsShowingFailedMessage
        {
            get => _isShowingFailedMessage;
            set
            {
                Param_Setter(ref _isShowingFailedMessage, value);
                FailedMesageVisibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public Visibility IsLoadingVisibility
        {
            get { return _isLoadingVisibility; }
            set { Param_Setter(ref _isLoadingVisibility, value); }
        }

        public Visibility FailedMesageVisibility
        {
            get { return _failedMesageVisibility; }
            set { Param_Setter(ref _failedMesageVisibility, value); }
        }

        public ICommand RefreshCommand => _refreshCommand ?? (_refreshCommand = new System.Windows.Input.ICommand(OnRefresh));

        public System.Windows.Input.ICommand BrowserBackCommand => _browserBackCommand ?? (_browserBackCommand = new System.Windows.Input.ICommand(() => _webView?.GoBack(), () => _webView?.CanGoBack ?? false));

        public System.Windows.Input.ICommand BrowserForwardCommand => _browserForwardCommand ?? (_browserForwardCommand = new System.Windows.Input.ICommand(() => _webView?.GoForward(), () => _webView?.CanGoForward ?? false));

        public ICommand OpenInBrowserCommand => _openInBrowserCommand ?? (_openInBrowserCommand = new System.Windows.Input.ICommand(OnOpenInBrowser));

        public wts.ItemNameViewModel()
        {
            Source = DefaultUrl;
        }

        public void Initialize(WebView webView)
        {
            _webView = webView;
        }

        public void OnNavigationCompleted(WebViewControlNavigationCompletedEventArgs e)
        {
            IsLoading = false;
            BrowserBackCommand.Param_CanExecuteChangedMethodName();
            BrowserForwardCommand.Param_CanExecuteChangedMethodName();
            if (e != null && !e.IsSuccess)
            {
                // Use `args.WebErrorStatus` to vary the displayed message based on the error reason
                IsShowingFailedMessage = true;
            }
        }

        private void OnRefresh()
        {
            IsShowingFailedMessage = false;
            IsLoading = true;
            _webView?.Refresh();
        }

        private void OnOpenInBrowser()
        {
            // There is an open Issue on this
            // https://github.com/dotnet/corefx/issues/10361
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = Source,
                UseShellExecute = true
            };
            Process.Start(psi);
        }
    }
}