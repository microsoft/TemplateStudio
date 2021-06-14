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

        public ICommand MenuViewswts.ItemNameCommand => _menuViewswts.ItemNameCommand ?? (_menuViewswts.ItemNameCommand = new RelayCommand(OnMenuViewswts.ItemName));
//}]}
        public INavigationService NavigationService { get; }
//^^
//{[{
        private void OnMenuViewswts.ItemName() => NavigationService.NavigateTo(typeof(wts.ItemNameViewModel).FullName, null, true);

//}]}
    }
}
