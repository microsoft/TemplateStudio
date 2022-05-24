namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : ObservableRecipient
    {
        private bool _isBackEnabled;
        private object _selected;
//{[{
        private ICommand _menuParam_ItemNameCommand;
//}]}
        public ICommand MenuFileExitCommand => _menuFileExitCommand ??= new RelayCommand(OnMenuFileExit);
//{[{

        public ICommand MenuParam_ItemNameCommand => _menuParam_ItemNameCommand ??= new RelayCommand(OnMenuParam_ItemName);
//}]}
        private void OnMenuFileExit() => Application.Current.Exit();
//{[{

        private void OnMenuParam_ItemName() => NavigationService.NavigateTo(typeof(Param_ItemNameViewModel).FullName);
//}]}
    }
}
