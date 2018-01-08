using System;

using Windows.UI.Xaml.Controls;

using XamarinUwpNative.UWP.ViewModels;

namespace XamarinUwpNative.UWP.Views
{
    public sealed partial class WebViewPage : Page
    {
        public WebViewViewModel ViewModel { get; } = new WebViewViewModel();

        public WebViewPage()
        {
            InitializeComponent();
            ViewModel.Initialize(webView);
        }
    }
}
