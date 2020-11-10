using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using WinUI3App.ViewModels;

namespace WinUI3App.Views
{
    public sealed partial class WebViewPage : Page
    {
        public WebViewViewModel ViewModel { get; }

        public WebViewPage()
        {
            //ViewModel = Ioc.Default.GetService<WebViewViewModel>();
            InitializeComponent();
            ViewModel = new WebViewViewModel(new Services.WebViewService(webView));
            //ViewModel.WebViewService.Initialize(webView);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }
    }
}
