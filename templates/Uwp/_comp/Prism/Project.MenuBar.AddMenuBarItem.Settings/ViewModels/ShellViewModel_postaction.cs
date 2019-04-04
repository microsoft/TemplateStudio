namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        public ICommand MenuFileExitCommand { get; }
// {[{

        public ICommand MenuFilewts.ItemNameCommand { get; }
//}]}
        {
            _menuNavigationService = menuNavigationService;
//^^
//{[{
            MenuFilewts.ItemNameCommand = new DelegateCommand(OnMenuFilewts.ItemName);
//}]}
        }

        private void OnMenuFileExit()
        {
        }
//^^
// {[{

        private void OnMenuFilewts.ItemName()
        {
            _menuNavigationService.OpenInRightPane(typeof(wts.ItemNamePage));
        }
//}]}
    }
}
