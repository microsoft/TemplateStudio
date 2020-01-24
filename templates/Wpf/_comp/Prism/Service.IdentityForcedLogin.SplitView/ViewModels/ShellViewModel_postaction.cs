//{[{
using Param_RootNamespace.Contracts.Services;
//}]}
namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : BindableBase
    {
//^^
//{[{
        private readonly IUserDataService _userDataService;
//}]}
        private IRegionNavigationService _navigationService;
        public ShellViewModel(/*{[{*/IUserDataService userDataService/*}]}*/)
        {
//^^
//{[{
            _userDataService = userDataService;
//}]}
        }

        private void OnLoaded()
        {
//^^
//{[{
            var user = _userDataService.GetUser();
            var userMenuItem = new HamburgerMenuImageItem()
            {
                Thumbnail = user.Photo,
                Label = user.Name,
                Command = new DelegateCommand(OnUserItemSelected)
            };

            OptionMenuItems.Insert(0, userMenuItem);
//}]}
        }
//^^
//{[{
        private void OnUserItemSelected()
            => RequestNavigate(PageKeys.Settings);
//}]}

        private void OnNavigated(object sender, RegionNavigationEventArgs e)
        {
        }
    }
}