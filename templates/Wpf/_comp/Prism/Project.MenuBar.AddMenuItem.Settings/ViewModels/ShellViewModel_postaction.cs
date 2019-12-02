//{[{
using Param_RootNamespace.Contracts.Services;
//}]}
namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : BindableBase, IDisposable
    {
//{[{
        private readonly IRightPaneService _rightPaneService;
//}]}
        private DelegateCommand _goBackCommand;
//{[{
        private ICommand _menuFilewts.ItemNameCommand;
//}]}

        public DelegateCommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new DelegateCommand(OnGoBack, CanGoBack));
//{[{

        public ICommand MenuFilewts.ItemNameCommand => _menuFilewts.ItemNameCommand ?? (_menuFilewts.ItemNameCommand = new DelegateCommand(OnMenuFilewts.ItemName));
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

        private void OnMenuFilewts.ItemName()
            => RequestNavigateOnRightPane(PageKeys.wts.ItemName);
//}]}
    }
}
