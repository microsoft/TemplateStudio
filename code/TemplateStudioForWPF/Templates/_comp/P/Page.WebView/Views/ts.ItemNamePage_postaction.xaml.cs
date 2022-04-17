//{[{
using Param_RootNamespace.ViewModels;
using Microsoft.Web.WebView2.Core;
using System.Windows;
//}]}

namespace Param_RootNamespace.Views
{
    public partial class ts.ItemNamePage : UserControl
    {
//{[{
        private ts.ItemNameViewModel ViewModel
            => DataContext as ts.ItemNameViewModel;

//}]}
        public ts.ItemNamePage()
        {

        }
//^^
//{[{

        private void OnNavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
            => ViewModel.OnNavigationCompleted(sender, e);

        private void ts.ItemNamePage_OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel?.Initialize(webView);
        }
//}]}
    }
}
