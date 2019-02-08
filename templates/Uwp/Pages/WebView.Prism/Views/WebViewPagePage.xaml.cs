using Windows.UI.Xaml.Controls;
using Param_RootNamespace.Services;

namespace Param_RootNamespace.Views
{
    public sealed partial class WebViewPagePage : Page
    {
        private WebViewPageViewModel ViewModel => DataContext as WebViewPageViewModel;

        public WebViewPagePage()
        {
            InitializeComponent();

            // This is an unusual way to initialize a service to a ViewModel, but since this service
            // requires a reference to the WebView this is one way to provide the required reference.
            // In this case teh WebViewService isn't a traditional Service but more of a shim to provide to better
            // separation of View and ViewModel and unit testing of a ViewModel that uses the WebViewService since the
            // WebViewService implements the IWebViewService interface that allows for mocking of the service.
            ViewModel.WebViewService = new WebViewService(webView);
        }
    }
}
