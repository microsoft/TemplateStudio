namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        public ICommand MenuFileExitCommand { get; }
// {[{

        public ICommand MenuViewswts.ItemNameCommand { get; }
//}]}
        {
            _menuNavigationService = menuNavigationService;
//^^
//{[{
            MenuViewswts.ItemNameCommand = new DelegateCommand(OnMenuViewswts.ItemName);
//}]}
        }

        private void OnMenuFileExit()
        {
        }
//^^
// {[{

        private void OnMenuViewswts.ItemName()
        {
            _menuNavigationService.UpdateView(PageTokens.wts.ItemNamePage);
        }
//}]}
    }
}
