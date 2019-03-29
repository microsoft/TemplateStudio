namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
//^^
// {[{
        public ICommand MenuFilewts.ItemNameCommand { get; }

//}]}
        public ShellViewModel(IMenuNavigationService menuNavigationService)
        {
//^^
//{[{
            MenuFilewts.ItemNameCommand = new DelegateCommand(OnMenuFilewts.ItemName);
//}]}
        }
//^^
// {[{
        private void OnMenuFilewts.ItemName()
        {
            _menuNavigationService.OpenInRightPane(typeof(wts.ItemNamePage));
        }

//}]}
        private void OnMenuFileExit()
        {
        }
    }
}
