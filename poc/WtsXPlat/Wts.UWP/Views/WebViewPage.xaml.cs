using System;

using Windows.UI.Xaml.Controls;

using Wts.UWP.ViewModels;

namespace Wts.UWP.Views
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
