namespace Param_RootNamespace.Views
{
    public partial class wts.ItemNamePage : Page
    {
//{[{
        private readonly wts.ItemNameViewModel _viewModel;
//}]}

        public wts.ItemNamePage(wts.ItemNameViewModel viewModel)
        {
//^^
//{[{
            _viewModel = viewModel;
            _viewModel.Initialize(webView);
//}]}
        }
//^^
//{[{

        private void OnNavigationCompleted(object sender, WebViewControlNavigationCompletedEventArgs e)
            => _viewModel.OnNavigationCompleted(e);
//}]}
    }
}
