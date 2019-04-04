//{[{
using Param_RootNamespace.Core.Helpers;
using Param_RootNamespace.Core.Services;
//}]}
namespace Param_RootNamespace.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
//{[{
        private IUserDataService _userDataService;
        private IIdentityService _identityService;
//}]}
        private ElementTheme _elementTheme = ThemeSelectorService.Theme;
//{[{
        private UserViewModel _user;

        public UserViewModel User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        public ICommand LogoutCommand;
//}]}

        public SettingsViewModel()
        {
//{[{
            _identityService = identityService;
            _userDataService = userDataService;
            LogoutCommand = new DelegateCommand(OnLogout);
//}]}
        }

        public async Task InitializeAsync()
        {
//^^
//{[{
            _identityService.LoggedOut += OnLoggeOut;
            _userDataService.UserDataUpdated += OnUserDataUpdated;
            User = await _userDataService.GetUserAsync();
//}]}
        }
//{[{

        public void UnregisterEvents()
        {
            _identityService.LoggedOut -= OnLoggeOut;
            _userDataService.UserDataUpdated -= OnUserDataUpdated;
        }

        private void OnUserDataUpdated(object sender, UserViewModel user)
        {
            User = user;
        }

        private async void OnLogout()
        {
            await _identityService.LogoutAsync();
        }

        private void OnLoggeOut(object sender, EventArgs e)
        {
            UnregisterEvents();
        }
//}]}
    }
}
