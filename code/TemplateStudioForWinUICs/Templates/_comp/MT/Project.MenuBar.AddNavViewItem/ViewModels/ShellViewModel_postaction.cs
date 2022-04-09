namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : ObservableRecipient
    {
        private bool _isBackEnabled;
        private object _selected;
//{[{
        private ICommand _menuViewsParam_ItemNameCommand;
//}]}
        public ICommand MenuFileExitCommand => _menuFileExitCommand ?? (_menuFileExitCommand = new RelayCommand(OnMenuFileExit));
//{[{

        public ICommand MenuViewsParam_ItemNameCommand => _menuViewsParam_ItemNameCommand ?? (_menuViewsParam_ItemNameCommand = new RelayCommand(OnMenuViewsParam_ItemName));
//}]}
        public INavigationService NavigationService { get; }
//^^
//{[{

        private void OnMenuViewsParam_ItemName() => NavigationService.NavigateTo(typeof(Param_ItemNameViewModel).FullName);
//}]}
    }
}
