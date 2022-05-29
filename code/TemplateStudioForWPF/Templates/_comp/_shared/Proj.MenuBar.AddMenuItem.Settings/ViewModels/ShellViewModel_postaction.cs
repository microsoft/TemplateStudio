namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;
        //{[{
        private readonly IRightPaneService _rightPaneService;
        //}]}
        private RelayCommand _goBackCommand;
        //{[{
        private ICommand _menuFilets.ItemNameCommand;
        //}]}
        private ICommand _loadedCommand;
        public System.Windows.Input.ICommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new System.Windows.Input.ICommand(OnGoBack, CanGoBack));
        //{[{

        public ICommand MenuFilets.ItemNameCommand => _menuFilets.ItemNameCommand ?? (_menuFilets.ItemNameCommand = new System.Windows.Input.ICommand(OnMenuFilets.ItemName));
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

        private void OnMenuFilets.ItemName()
            => _rightPaneService.OpenInRightPane(typeof(ts.ItemNameViewModel).FullName);
        //}]}
    }
}
