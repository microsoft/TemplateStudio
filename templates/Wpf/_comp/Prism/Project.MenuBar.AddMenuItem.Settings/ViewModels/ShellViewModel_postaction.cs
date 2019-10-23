namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : BindableBase, IDisposable
    {
        private IRegionNavigationService _navigationService;
//{[{
        private IRegionNavigationService _rightPanenavigationService;
//}]}
        private DelegateCommand _goBackCommand;
//{[{
        private ICommand _menuFilewts.ItemNameCommand;
//}]}

        public DelegateCommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new DelegateCommand(OnGoBack, CanGoBack));

//{[{
        public ICommand MenuFilewts.ItemNameCommand => _menuFilewts.ItemNameCommand ?? (_menuFilewts.ItemNameCommand = new DelegateCommand(OnMenuFilewts.ItemName));
//}]}
        private void OnLoaded()
        {
            _navigationService = _regionManager.Regions[Regions.Main].NavigationService;
//{[{
            _rightPanenavigationService = _regionManager.Regions[Regions.RightPane].NavigationService;
//}]}
        }

        private bool RequestNavigate(string target)
        {
        }
//{[{

        private bool RequestNavigateOnRightPane(string target)
        {
            if (_rightPanenavigationService.CanNavigate(target))
            {
                _rightPanenavigationService.RequestNavigate(target);
                return true;
            }

            return false;
        }
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
