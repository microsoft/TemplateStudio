namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : ObservableRecipient
    {
        private bool _isBackEnabled;
        private object _selected;
//{[{
        private ICommand _menuFilewts.ItemNameCommand;
//}]}
        public ICommand MenuFileExitCommand => _menuFileExitCommand ?? (_menuFileExitCommand = new RelayCommand(OnMenuFileExit));
//{[{

        public ICommand MenuFilewts.ItemNameCommand => _menuFilewts.ItemNameCommand ?? (_menuFilewts.ItemNameCommand = new RelayCommand(OnMenuFilewts.ItemName));
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

        private void OnMenuFilewts.ItemName() => RightPaneService.OpenInRightPane(typeof(wts.ItemNameViewModel).FullName);
//}]}
    }
}
