namespace Param_RootNamespace.ViewModels;

public partial class ShellViewModel : ObservableRecipient
{
    public ICommand MenuFileExitCommand
    {
        get;
    }
//{[{
    public ICommand MenuViewsParam_ItemNameCommand
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
        MenuViewsParam_ItemNameCommand = new RelayCommand(OnMenuViewsParam_ItemName);
//}]}
    }

    private void OnMenuFileExit() => Application.Current.Exit();
//{[{

    private void OnMenuViewsParam_ItemName() => NavigationService.NavigateTo(typeof(Param_ItemNameViewModel).FullName!);
//}]}
}
