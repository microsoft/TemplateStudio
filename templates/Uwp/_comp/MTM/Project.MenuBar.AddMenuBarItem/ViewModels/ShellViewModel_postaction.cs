namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : ObservableObject
    {
        private IList<KeyboardAccelerator> _keyboardAccelerators;
//^^
//{[{
        private ICommand _menuViewswts.ItemNameCommand;
//}]}
        private ICommand _menuFileExitCommand;
//^^
//{[{
        public ICommand MenuViewswts.ItemNameCommand => _menuViewswts.ItemNameCommand ?? (_menuViewswts.ItemNameCommand = new RelayCommand(OnMenuViewswts.ItemName));

//}]}
        public ICommand MenuFileExitCommand => _menuFileExitCommand ?? (_menuFileExitCommand = new RelayCommand(OnMenuFileExit));

        public ShellViewModel()
        {
        }
//^^
//{[{
        private void OnMenuViewswts.ItemName() => MenuNavigationHelper.UpdateView(typeof(wts.ItemNamePage));

//}]}
        private void OnMenuFileExit()
        {
        }
    }
}
