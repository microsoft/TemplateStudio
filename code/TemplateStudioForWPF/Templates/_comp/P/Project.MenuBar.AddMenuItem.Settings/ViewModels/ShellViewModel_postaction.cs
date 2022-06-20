//{[{
using Param_RootNamespace.Contracts.Services;
//}]}
namespace Param_RootNamespace.ViewModels;

public class ShellViewModel : BindableBase
{
//{[{
    private readonly IRightPaneService _rightPaneService;
//}]}
    private DelegateCommand _goBackCommand;
//{[{
    private ICommand _menuFilets.ItemNameCommand;
//}]}
    private ICommand _loadedCommand;
    public DelegateCommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new DelegateCommand(OnGoBack, CanGoBack));
//{[{

    public ICommand MenuFilets.ItemNameCommand => _menuFilets.ItemNameCommand ?? (_menuFilets.ItemNameCommand = new DelegateCommand(OnMenuFilets.ItemName));
//}]}
    public ShellViewModel(/*{[{*/IRightPaneService rightPaneService/*}]}*/)
    {
//^^
//{[{
        _rightPaneService = rightPaneService;
//}]}
    }

    private bool RequestNavigate(string target)
    {
    }
//{[{

    private void RequestNavigateOnRightPane(string target)
        => _rightPaneService.OpenInRightPane(target);
//}]}
    private void RequestNavigateAndCleanJournal(string target)
    {
    }
//^^
//{[{

    private void OnMenuFilets.ItemName()
        => RequestNavigateOnRightPane(PageKeys.ts.ItemName);
//}]}
}
