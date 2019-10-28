namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : System.ComponentModel.INotifyPropertyChanged, IDisposable
    {
        private RelayCommand _goBackCommand;
//{[{
        private ICommand _menuViewswts.ItemNameCommand;
//}]}

        public System.Windows.Input.ICommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new System.Windows.Input.ICommand(OnGoBack, CanGoBack));
//{[{

        public ICommand MenuViewswts.ItemNameCommand => _menuViewswts.ItemNameCommand ?? (_menuViewswts.ItemNameCommand = new System.Windows.Input.ICommand(OnMenuViewswts.ItemName));
//}]}

//^^
//{[{
        private void OnMenuViewswts.ItemName()
            => _navigationService.NavigateTo(typeof(wts.ItemNameViewModel).FullName, null, true);
//}]}
    }
}
