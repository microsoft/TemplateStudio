using System;

using Windows.UI.Xaml.Controls;

using WtsXamarinUWP.UWP.ViewModels;

namespace WtsXamarinUWP.UWP.Views
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
