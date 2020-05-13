
//{[{
using Param_RootNamespace.ViewModels;
//}]}

namespace Param_RootNamespace.Views
{
    public partial class wts.ItemNamePage : Page
    {
//{[{
        private wts.ItemNameViewModel ViewModel
            => DataContext as wts.ItemNameViewModel;

//}]}
        public wts.ItemNamePage()
        {
//^^
//{[{
            ViewModel.Initialize(webView);
//}]}
        }
//^^
//{[{

        private void OnNavigationCompleted(object sender, WebViewControlNavigationCompletedEventArgs e)
            => ViewModel.OnNavigationCompleted(e);
//}]}
    }
}
