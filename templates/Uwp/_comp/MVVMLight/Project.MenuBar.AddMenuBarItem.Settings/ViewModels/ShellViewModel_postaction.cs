namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        private IList<KeyboardAccelerator> _keyboardAccelerators;
//^^
//{[{
        private ICommand _menuFilewts.ItemNameCommand;
//}]}
        private ICommand _menuFileExitCommand;
//^^
//{[{
        public ICommand MenuFilewts.ItemNameCommand => _menuFilewts.ItemNameCommand ?? (_menuFilewts.ItemNameCommand = new RelayCommand(OnMenuFilewts.ItemName));

//}]}
        public ICommand MenuFileExitCommand => _menuFileExitCommand ?? (_menuFileExitCommand = new RelayCommand(OnMenuFileExit));

        public ShellViewModel()
        {
        }
//^^
//{[{
        private void OnMenuFilewts.ItemName() => MenuNavigationHelper.OpenInRightPane(typeof(Views.wts.ItemNamePage));

//}]}
        private void OnMenuFileExit()
        {
        }
    }
}
