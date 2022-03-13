//{[{
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

        public ICommand LogoutCommand { get; }
//}]}
        public SettingsViewModel(/*{[{*/IIdentityService identityService, IUserDataService userDataService/*}]}*/)
        {
//^^
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
            _identityService.LoggedOut += OnLoggedOut;
            _userDataService.UserDataUpdated += OnUserDataUpdated;
            User = await _userDataService.GetUserAsync();
//}]}
        }
//{[{

        public void UnregisterEvents()
        {
            _identityService.LoggedOut -= OnLoggedOut;
            _userDataService.UserDataUpdated -= OnUserDataUpdated;
        }

        private void OnUserDataUpdated(object sender, UserViewModel userData)
        {
            User = userData;
        }

        private async void OnLogout()
        {
            await _identityService.LogoutAsync();
        }

        private void OnLoggedOut(object sender, EventArgs e)
        {
            UnregisterEvents();
        }
//}]}
    }
}
