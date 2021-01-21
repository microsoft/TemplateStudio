using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

using WinUIDesktopApp.ViewModels;

namespace WinUIDesktopApp.Views
{
    // To learn more about WebView2, see https://docs.microsoft.com/microsoft-edge/webview2/
    public sealed partial class WebViewPage : Page
    {
        public WebViewViewModel ViewModel { get; }

        public WebViewPage()
        {
            ViewModel = Ioc.Default.GetService<WebViewViewModel>();
            InitializeComponent();
            ViewModel.WebViewService.Initialize(webView);
        }
    }
}
