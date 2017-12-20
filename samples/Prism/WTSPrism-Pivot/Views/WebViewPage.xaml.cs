using Windows.UI.Xaml.Controls;
using WTSPrism.Services;
using WTSPrism.ViewModels;

namespace WTSPrism.Views
{
    public sealed partial class WebViewPage : Page
    {
        private WebViewPageViewModel ViewModel
        {
            get { return DataContext as WebViewPageViewModel; }
        }

        public WebViewPage()
        {
            InitializeComponent();

            // This is an unusual way to initialize a service to a ViewModel, but since this service
            // requires a reference to the WebView this is one way to provide the required reference.
            // In this case the WebViewService isn't a traditional Service but more of a shim to provide to better
            // separation of View and ViewModel and unit testing of a ViewModel that uses the WebViewService since the
            // WebViewService implements the IWebViewService interface that allows for mocking of the service.
            ViewModel.WebViewService = new WebViewService(this.webView);
        }
    }
}
