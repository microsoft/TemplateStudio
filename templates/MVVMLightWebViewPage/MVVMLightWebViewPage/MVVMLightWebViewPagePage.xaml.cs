using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Page_NS.MVVMLightWebViewPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MVVMLightWebViewPagePage : Page
    {
        public MVVMLightWebViewPagePage()
        {
            this.InitializeComponent();
            ViewModel = new MVVMLightWebViewPageViewModel();
            DataContext = ViewModel;
        }
        public MVVMLightWebViewPageViewModel ViewModel { get; private set; }

        private void OnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            ViewModel.LoadingIndicatorVisibility = Visibility.Collapsed;
        }

        private void OnNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            ViewModel.LoadingIndicatorVisibility = Visibility.Visible;
        }
    }
}
