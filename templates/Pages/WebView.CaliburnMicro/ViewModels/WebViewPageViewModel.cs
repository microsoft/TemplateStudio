using System;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Param_ItemNamespace.Helpers;
using Param_ItemNamespace.Extensions;
using Param_ItemNamespace.Models;
using Caliburn.Micro;

namespace Param_ItemNamespace.ViewModels
{
    public class WebViewPageViewModel : Screen
    {
        // TODO WTS: Specify your hyperlink default here
        private const string DefaultUrl = "https://developer.microsoft.com/en-us/windows/apps";
        private DataTransferManager _dataTransferManager;

        private Uri _source;

        public Uri Source
        {
            get { return _source; }
            set { Set(ref _source, value); }
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

                Set(ref _isLoading, value);
                IsLoadingVisibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private Visibility _isLoadingVisibility;

        public Visibility IsLoadingVisibility
        {
            get { return _isLoadingVisibility; }
            set { Set(ref _isLoadingVisibility, value); }
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

                Set(ref _isShowingFailedMessage, value);
                FailedMesageVisibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private Visibility _failedMesageVisibility;

        public Visibility FailedMesageVisibility
        {
            get { return _failedMesageVisibility; }
            set { Set(ref _failedMesageVisibility, value); }
        }

        public void NavCompleted(WebViewNavigationCompletedEventArgs e)
        {
            IsLoading = false;

            NotifyOfPropertyChange(() => CanGoBack);
            NotifyOfPropertyChange(() => CanGoForward);
        }

        public void NavFailed(WebViewNavigationFailedEventArgs e)
        {
            // Use `e.WebErrorStatus` to vary the displayed message based on the error reason
            IsShowingFailedMessage = true;

            NotifyOfPropertyChange(() => CanGoBack);
            NotifyOfPropertyChange(() => CanGoForward);
        }

        public void Retry()
        {
            IsShowingFailedMessage = false;
            IsLoading = true;

            _webView?.Refresh();
        }

        public void GoBack() => _webView?.GoBack();

        public bool CanGoBack => _webView == null ? false : _webView.CanGoBack;

        public void GoForward() => _webView?.GoForward();

        public bool CanGoForward => _webView?.CanGoForward ?? false;

        public void RefreshBrowser() => _webView?.Refresh();

        public void Share() => DataTransferManager.ShowShareUI();

        public async void OpenInBrowser() => await Windows.System.Launcher.LaunchUriAsync(Source);

        private WebView _webView;

        public WebViewPageViewModel()
        {
            IsLoading = true;
            Source = new Uri(DefaultUrl);
            _dataTransferManager = DataTransferManager.GetForCurrentView();
            _dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(OnDataRequested);
        }

        public void Initialize(WebView webView)
        {
            _webView = webView;
        }

        public void RegisterEvents()
        {
            _dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(OnDataRequested);
        }

        public void UnregisterEvents()
        {
            _dataTransferManager.DataRequested -= new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(OnDataRequested);
        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            // This event will be fired when the share operation starts.
            // We need to add data to DataRequestedEventArgs through SetData extension method
            var config = new ShareSourceData("Sharing a web link");

            // TODO WTS: Use ShareSourceConfig instance to set the data you want to share
            config.SetWebLink(Source);

            args.Request.SetData(config);
            args.Request.Data.ShareCompleted += OnShareCompleted;
        }

        private void OnShareCompleted(DataPackage sender, ShareCompletedEventArgs args)
        {
            // This event will be fired when Share Operation will finish
            // TODO WTS: If you need to handle any action when de data is shared implement on this method
            sender.ShareCompleted -= OnShareCompleted;
        }
    }
}
