//{[{
using Param_RootNamespace.Contracts.Services;
using MahApps.Metro.Controls;
//}]}
namespace Param_RootNamespace.Views
{
    public partial class ts.ItemNamePage : Page, INotifyPropertyChanged, INavigationAware
    {
        private const string DefaultUrl = "https://docs.microsoft.com/windows/apps/";
//{[{
        private readonly IRightPaneService _rightPaneService;
//}]}
        public ts.ItemNamePage(/*{[{*/IRightPaneService rightPaneService/*}]}*/)
        {
//^^
//{[{
            _rightPaneService = rightPaneService;
//}]}
        }

        public void OnNavigatedTo(object parameter)
        {
//^^
//{[{
            _rightPaneService.PaneOpened += OnRightPaneOpened;
            _rightPaneService.PaneClosed += OnRightPaneClosed;
//}]}
        }

        public void OnNavigatedFrom()
        {
//^^
//{[{
        _rightPaneService.PaneOpened -= OnRightPaneOpened;
        _rightPaneService.PaneClosed -= OnRightPaneClosed;
//}]}
        }
//{[{

        private void OnRightPaneOpened(object sender, System.EventArgs e)
        {
            // WebView control is always rendered on top
            // We need to adapt the WebView to be able to show the right pane
            if (sender is SplitView splitView)
            {
                webView.Margin = new Thickness(0, 0, splitView.OpenPaneLength, 0);
            }
        }

        private void OnRightPaneClosed(object sender, System.EventArgs e)
         => webView.Margin = new Thickness(0);
//}]}
        private void OnNavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
        }
    }
}
