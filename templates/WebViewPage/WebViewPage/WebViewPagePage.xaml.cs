using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#if (isMVVMLight)
using Microsoft.Practices.ServiceLocation;
#endif

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Param_PageNS.WebViewPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WebViewPagePage : Page
    {
        public WebViewPagePage()
        {
            this.InitializeComponent();
#if (isBasic)
            ViewModel = new WebViewPageViewModel();
            DataContext = ViewModel;
#endif
        }
#if (isBasic)
        public WebViewPageViewModel ViewModel { get; private set; }

        private void OnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            ViewModel.LoadingIndicatorVisibility = Visibility.Collapsed;
        }

        private void OnNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            ViewModel.LoadingIndicatorVisibility = Visibility.Visible;
        }
#else if(isMVVMLight)
        private void OnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            var viewModel = ServiceLocator.Current.GetInstance<WebViewPageViewModel>();
            viewModel.LoadingIndicatorVisibility = Visibility.Collapsed;
        }

        private void OnNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            var viewModel = ServiceLocator.Current.GetInstance<WebViewPageViewModel>();
            viewModel.LoadingIndicatorVisibility = Visibility.Visible;
        }
#endif
    }
}
