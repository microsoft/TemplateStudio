namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
//^^
// {[{
        public ICommand MenuViewswts.ItemNameCommand { get; }

//}]}
//{??{
        public ShellViewModel(IMenuNavigationService menuNavigationService)
//}??}
        {
            _menuNavigationService = menuNavigationService;
//^^
//{[{
            MenuViewswts.ItemNameCommand = new DelegateCommand(OnMenuViewswts.ItemName);
//}]}
        }
//^^
// {[{
        private void OnMenuViewswts.ItemName()
        {
            _menuNavigationService.UpdateView(PageTokens.wts.ItemNamePage);
        }

//}]}
        private void OnMenuFileExit()
        {
        }
    }
}
