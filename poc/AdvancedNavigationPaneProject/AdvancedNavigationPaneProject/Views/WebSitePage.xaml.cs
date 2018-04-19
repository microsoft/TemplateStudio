using System;

using AdvancedNavigationPaneProject.ViewModels;

using Windows.UI.Xaml.Controls;

namespace AdvancedNavigationPaneProject.Views
{
    public sealed partial class WebSitePage : Page
    {
        public WebSiteViewModel ViewModel { get; } = new WebSiteViewModel();

        public WebSitePage()
        {
            InitializeComponent();
            ViewModel.Initialize(webView);
        }
    }
}
