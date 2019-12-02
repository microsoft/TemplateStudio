//{[{
using Param_RootNamespace.Contracts.Services;
using MahApps.Metro.Controls;
//}]}
namespace Param_RootNamespace.ViewModels
{
    public class wts.ItemNameViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private const string DefaultUrl = "https://docs.microsoft.com/windows/apps/";
//{[{
        private readonly IRightPaneService _rightPaneService;
//}]}
        public wts.ItemNameViewModel(/*{[{*/IRightPaneService rightPaneService/*}]}*/)
        {
//^^
//{[{
            _rightPaneService = rightPaneService;
//}]}
        }

        public void Initialize(WebView webView)
        {
//^^
//{[{
            _rightPaneService.PaneOpened += OnRightPaneOpened;
            _rightPaneService.PaneClosed += OnRightPaneClosed;
//}]}
        }
//^^
//{[{

        private void OnRightPaneOpened(object sender, System.EventArgs e)
        {
            // WebView control is always rendered on top
            // We need to adapt the WebView to be able to show the right pane
            if (sender is SplitView splitView)
            {
                _webView.Margin = new Thickness(0, 0, splitView.OpenPaneLength, 0);
            }
        }

        private void OnRightPaneClosed(object sender, System.EventArgs e)
         => _webView.Margin = new Thickness(0);
//}]}
    }
}
