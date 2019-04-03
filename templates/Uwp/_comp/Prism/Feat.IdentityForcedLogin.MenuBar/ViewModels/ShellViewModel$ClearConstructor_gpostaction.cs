namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
//{--{
        public ShellViewModel(IMenuNavigationService menuNavigationService)
        {
            _menuNavigationService = menuNavigationService;
            MenuFileExitCommand = new DelegateCommand(OnMenuFileExit);
            MenuViewsMainCommand = new DelegateCommand(OnMenuViewsMain);
            MenuFileSettingsCommand = new DelegateCommand(OnMenuFileSettings);
        }
//}--}
    }
}