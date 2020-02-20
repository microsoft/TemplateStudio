//{[{
using Param_RootNamespace.Constants;
//}]}
namespace Param_RootNamespace.ViewModels
{
    public class SettingsViewModel : BindableBase, INavigationAware
    {
//^^
//{[{
        private readonly IRegionManager _regionManager;
        private readonly IRegionNavigationService _navigationService;
//}]}
        private readonly ISystemService _systemService;

        public SettingsViewModel(/*{[{*/IRegionManager regionManager/*}]}*/)
        {
//^^
//{[{
            _regionManager = regionManager;
            _navigationService = _regionManager.Regions[Regions.Main].NavigationService;
//}]}
        }

        private void OnLoggedOut(object sender, EventArgs e)
        {
//^^
//{[{
            _navigationService.RequestNavigate(PageKeys.Main);
            _navigationService.Journal.Clear();
//}]}
        }
    }
}