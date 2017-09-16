using Windows.UI.Xaml.Controls;
using WTSPrism.Services;
using WTSPrism.ViewModels;

namespace WTSPrism.Views
{
    public sealed partial class WebViewPage : Page
    {
        private WebViewPageViewModel ViewModel => DataContext as WebViewPageViewModel;

        public WebViewPage()
        {
            InitializeComponent();
            ViewModel.WebViewService = new WebViewService(this.webView);
        }
    }
}
