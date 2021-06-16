namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : ObservableRecipient
    {
        private bool _isBackEnabled;
        private object _selected;
//{[{
        private ICommand _menuViewswts.ItemNameCommand;
//}]}
        public ICommand MenuFileExitCommand => _menuFileExitCommand ?? (_menuFileExitCommand = new RelayCommand(OnMenuFileExit));
//{[{

        public ICommand MenuFileswts.ItemNameCommand => _menuViewswts.ItemNameCommand ?? (_menuViewswts.ItemNameCommand = new RelayCommand(OnMenuFileswts.ItemName));
//}]}
        public ShellViewModel(/*{[{*/IRightPaneService rightPaneService/*}]}*/)
        {
//^^
//{[{
            RightPaneService = rightPaneService;
//}]}
        }
//^^
//{[{

        private void OnMenuFileswts.ItemName() => RightPaneService.OpenInRightPane(typeof(wts.ItemNameViewModel).FullName);
//}]}
    }
}
