namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
//^^
//{[{
        public ICommand MenuViewswts.ItemNameCommand { get; }

//}]}
        public ICommand MenuFileExitCommand { get; }

        private void OnMenuFileExit()
        {
        }
//^^
//{[{

        private void OnMenuViewswts.ItemName()
        {
            _menuNavigationService.UpdateView(PageTokens.wts.ItemNamePage);
        }
//}]}
    }
}
