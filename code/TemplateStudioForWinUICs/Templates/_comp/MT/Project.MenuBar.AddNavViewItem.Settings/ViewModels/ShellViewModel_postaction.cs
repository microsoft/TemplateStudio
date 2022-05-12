namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : ObservableRecipient
    {
        private bool _isBackEnabled;
        private object _selected;
//{[{
        private ICommand _menuFileParam_ItemNameCommand;
//}]}
        public ICommand MenuFileExitCommand => _menuFileExitCommand ??= new RelayCommand(OnMenuFileExit);
//{[{

        public ICommand MenuFileParam_ItemNameCommand => _menuFileParam_ItemNameCommand ??= new RelayCommand(OnMenuFileParam_ItemName);
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

        private void OnMenuFileParam_ItemName() => RightPaneService.OpenInRightPane(typeof(Param_ItemNameViewModel).FullName);
//}]}
    }
}
