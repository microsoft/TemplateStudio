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
            _userDataService.UserDataUpdated += OnUserDataUpdated;
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

        private void OnUnloaded()
        {
//^^
//{[{
            _userDataService.UserDataUpdated -= OnUserDataUpdated;
//}]}
        }
//{[{
        private void OnUserDataUpdated(object sender, UserViewModel user)
        {
            var userMenuItem = OptionMenuItems.OfType<HamburgerMenuImageItem>().FirstOrDefault();
            if (userMenuItem != null)
            {
                userMenuItem.Label = user.Name;
                userMenuItem.Thumbnail = user.Photo;
            }
        }

//}]}
//^^
//{[{
        private void OnUserItemSelected()
            => RequestNavigate(PageKeys.Settings);
//}]}

        private void RequestNavigate(string target)
        {
        }
    }
}