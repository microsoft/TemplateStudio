namespace Param_RootNamespace.ViewModels;

public partial class ShellViewModel : ObservableRecipient
{
    public ICommand MenuFileExitCommand
    {
        get;
    }
//{[{
    public ICommand MenuParam_ItemNameCommand
    {
        get;
    }
//}]}
    public ShellViewModel(INavigationService navigationService)
    {
        NavigationService = navigationService;
        NavigationService.Navigated += OnNavigated;

        MenuFileExitCommand = new RelayCommand(OnMenuFileExit);
//{[{
        MenuParam_ItemNameCommand = new RelayCommand(OnMenuParam_ItemName);
//}]}
    }

    private void OnMenuFileExit() => Application.Current.Exit();
//{[{

    private void OnMenuParam_ItemName() => NavigationService.NavigateTo(typeof(Param_ItemNameViewModel).FullName!);
//}]}
}
