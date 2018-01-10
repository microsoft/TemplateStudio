using System;

using Windows.UI.Xaml.Controls;

using WtsXPlat.UWP.ViewModels;

namespace WtsXPlat.UWP.Views
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
