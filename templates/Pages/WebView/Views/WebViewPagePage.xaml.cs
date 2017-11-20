using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Param_ItemNamespace.Views
{
    public sealed partial class WebViewPagePage : Page
    {
        public WebViewPagePage()
        {
            InitializeComponent();
            ViewModel.Initialize(webView);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.RegisterEvents();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            ViewModel.UnregisterEvents();
        }
    }
}
