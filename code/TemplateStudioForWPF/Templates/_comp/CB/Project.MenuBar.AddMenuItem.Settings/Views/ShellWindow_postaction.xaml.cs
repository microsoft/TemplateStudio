namespace Param_RootNamespace.Views
{
    public partial class ShellWindow : MetroWindow, IShellWindow, INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;
//{[{
        private readonly IRightPaneService _rightPaneService;
//}]}
        public ShellWindow(/*{[{*/IRightPaneService rightPaneService/*}]}*/)
        {
//^^
//{[{
            _rightPaneService = rightPaneService;
//}]}
        }
//^^
//{[{
        private void OnMenuFilewts.ItemName(object sender, RoutedEventArgs e)
            => _rightPaneService.OpenInRightPane(typeof(wts.ItemNamePage));
//}]}

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
