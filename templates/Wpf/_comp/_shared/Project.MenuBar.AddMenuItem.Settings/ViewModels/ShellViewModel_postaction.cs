namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : System.ComponentModel.INotifyPropertyChanged, IDisposable
    {
        private readonly INavigationService _navigationService;
//{[{
        private readonly IRightPaneService _rightPaneService;
//}]}
        private RelayCommand _goBackCommand;
//{[{
        private ICommand _menuFilewts.ItemNameCommand;
//}]}

        public System.Windows.Input.ICommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new System.Windows.Input.ICommand(OnGoBack, CanGoBack));
//{[{

        public ICommand MenuFilewts.ItemNameCommand => _menuFilewts.ItemNameCommand ?? (_menuFilewts.ItemNameCommand = new System.Windows.Input.ICommand(OnMenuFilewts.ItemName));
//}]}
        public ShellViewModel(/*{[{*/IRightPaneService rightPaneService/*}]}*/)
        {
//^^
//{[{
            _rightPaneService = rightPaneService;
//}]}
        }
//^^
//{[{

        private void OnMenuFilewts.ItemName()
            => _rightPaneService.OpenInRightPane(typeof(wts.ItemNameViewModel).FullName);
//}]}
    }
}
