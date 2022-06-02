namespace Param_RootNamespace.ViewModels;

public class ShellViewModel : ObservableRecipient
{
    private ICommand _menuFileExitCommand;
//{[{
    private ICommand _menuViewsParam_ItemNameCommand;
//}]}
    public ICommand MenuFileExitCommand => _menuFileExitCommand ??= new RelayCommand(OnMenuFileExit);
//{[{

    public ICommand MenuViewsParam_ItemNameCommand => _menuViewsParam_ItemNameCommand ??= new RelayCommand(OnMenuViewsParam_ItemName);
//}]}
    private void OnMenuFileExit() => Application.Current.Exit();
//{[{

    private void OnMenuViewsParam_ItemName() => NavigationService.NavigateTo(typeof(Param_ItemNameViewModel).FullName);
//}]}
}
