using Windows.UI.Xaml.Controls;
using WTSPrismNavigationBase.Services;
using WTSPrismNavigationBase.ViewModels;

namespace WTSPrismNavigationBase.Views
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
            ViewModel.WebViewService = new WebViewService(this.webView);
        }
    }
}
