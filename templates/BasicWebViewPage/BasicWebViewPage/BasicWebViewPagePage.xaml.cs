using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Page_NS.BasicWebViewPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BasicWebViewPagePage : Page
    {
        public BasicWebViewPagePage()
        {
            this.InitializeComponent();
            ViewModel = new BasicWebViewPageViewModel();
            DataContext = ViewModel;
        }
        public BasicWebViewPageViewModel ViewModel { get; private set; }

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
