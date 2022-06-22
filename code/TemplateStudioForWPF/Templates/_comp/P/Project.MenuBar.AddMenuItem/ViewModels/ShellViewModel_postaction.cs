namespace Param_RootNamespace.ViewModels;

public class ShellViewModel : BindableBase
{
    private DelegateCommand _goBackCommand;
    //{[{
    private ICommand _menuViewsts.ItemNameCommand;
    //}]}
    private ICommand _loadedCommand;
    public DelegateCommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new DelegateCommand(OnGoBack, CanGoBack));
    //{[{

    public ICommand MenuViewsts.ItemNameCommand => _menuViewsts.ItemNameCommand ?? (_menuViewsts.ItemNameCommand = new DelegateCommand(OnMenuViewsts.ItemName));
    //}]}
    private void OnGoBack()
        => _navigationService.Journal.GoBack();
    //^^
    //{[{

    private void OnMenuViewsts.ItemName()
        => RequestNavigateAndCleanJournal(PageKeys.ts.ItemName);
    //}]}
}
