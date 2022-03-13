namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
//^^
//{[{
        public ICommand MenuFilewts.ItemNameCommand { get; }

//}]}
        public ICommand MenuFileExitCommand { get; }

        private void OnMenuFileExit()
        {
        }
//^^
//{[{

        private void OnMenuFilewts.ItemName()
        {
            _menuNavigationService.OpenInRightPane(typeof(wts.ItemNamePage));
        }
//}]}
    }
}
