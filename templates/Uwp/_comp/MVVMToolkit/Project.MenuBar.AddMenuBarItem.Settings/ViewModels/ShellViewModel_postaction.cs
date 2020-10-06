namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : ObservableObject
    {
        private IList<KeyboardAccelerator> _keyboardAccelerators;
//^^
//{[{
        private ICommand _menuFileswts.ItemNameCommand;
//}]}
        private ICommand _menuFileExitCommand;
//^^
//{[{
        public ICommand MenuFilewts.ItemNameCommand => _menuFileswts.ItemNameCommand ?? (_menuFileswts.ItemNameCommand = new RelayCommand(OnMenuFilewts.ItemName));

//}]}
        public ICommand MenuFileExitCommand => _menuFileExitCommand ?? (_menuFileExitCommand = new RelayCommand(OnMenuFileExit));

        public ShellViewModel()
        {
        }
//^^
//{[{
        private void OnMenuFilewts.ItemName() => MenuNavigationHelper.OpenInRightPane(typeof(wts.ItemNamePage));

//}]}
        private void OnMenuFileExit()
        {
        }
    }
}
