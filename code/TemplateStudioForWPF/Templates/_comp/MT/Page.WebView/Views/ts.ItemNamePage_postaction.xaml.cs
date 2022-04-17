//{[{
using Microsoft.Web.WebView2.Core;
//}]}

namespace Param_RootNamespace.Views
{
    public partial class ts.ItemNamePage : Page
    {
//{[{
        private readonly ts.ItemNameViewModel _viewModel;
//}]}

        public ts.ItemNamePage(ts.ItemNameViewModel viewModel)
        {
//^^
//{[{
            _viewModel = viewModel;
            _viewModel.Initialize(webView);
//}]}
        }
//^^
//{[{

        private void OnNavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
            => _viewModel.OnNavigationCompleted(sender, e);
//}]}
    }
}
