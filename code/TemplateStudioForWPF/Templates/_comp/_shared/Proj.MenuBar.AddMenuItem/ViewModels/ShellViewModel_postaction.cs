namespace Param_RootNamespace.ViewModels;

public class ShellViewModel : System.ComponentModel.INotifyPropertyChanged
{
    private RelayCommand _goBackCommand;
    //{[{
    private ICommand _menuViewsts.ItemNameCommand;
    //}]}
    private ICommand _loadedCommand;
    public System.Windows.Input.ICommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new System.Windows.Input.ICommand(OnGoBack, CanGoBack));
    //{[{

    public ICommand MenuViewsts.ItemNameCommand => _menuViewsts.ItemNameCommand ?? (_menuViewsts.ItemNameCommand = new System.Windows.Input.ICommand(OnMenuViewsts.ItemName));
    //}]}

    //^^
    //{[{
    private void OnMenuViewsts.ItemName()
        => _navigationService.NavigateTo(typeof(ts.ItemNameViewModel).FullName, null, true);
    //}]}
}
