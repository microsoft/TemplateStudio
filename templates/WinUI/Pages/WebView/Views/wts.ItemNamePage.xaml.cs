using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Automation;
using Microsoft.UI.Xaml.Controls;
using Param_RootNamespace.Helpers;
using Param_RootNamespace.ViewModels;

namespace Param_RootNamespace.Views
{
    // To learn more about WebView2, see https://docs.microsoft.com/microsoft-edge/webview2/
    public sealed partial class wts.ItemNamePage : Page
    {
        public wts.ItemNameViewModel ViewModel { get; }

        public wts.ItemNamePage()
        {
            ViewModel = Ioc.Default.GetService<wts.ItemNameViewModel>();
            InitializeComponent();
            ViewModel.WebViewService.Initialize(webView);
            AutomationProperties.SetName(BrowserBackButton, "WebView_BrowserBackButtonAutomationPropertiesName".GetLocalized());
            AutomationProperties.SetName(BrowserForwardButton, "WebView_BrowserForwardButtonAutomationPropertiesName".GetLocalized());
            AutomationProperties.SetName(ReloadButton, "WebView_ReloadButtonAutomationPropertiesName".GetLocalized());
            AutomationProperties.SetName(OpenInBrowserButton, "WebView_OpenInBrowserButtonAutomationPropertiesName".GetLocalized());
        }
    }
}