//{[{
using Param_RootNamespace.Core.Helpers;
using Param_RootNamespace.Core.Services;
//}]}
namespace Param_RootNamespace.ViewModels
{
    public class SettingsViewModel : System.ComponentModel.INotifyPropertyChanged
    {
//{[{
        private UserDataService UserDataService => Singleton<UserDataService>.Instance;

        private IdentityService IdentityService => Singleton<IdentityService>.Instance;

//}]}
        private ElementTheme _elementTheme = ThemeSelectorService.Theme;
//{[{
        private UserViewModel _user;
//}]}

//^^
//{[{
        private ICommand _logoutCommand;

        public ICommand LogoutCommand => _logoutCommand ?? (_logoutCommand = new RelayCommand(OnLogout));

        public UserViewModel User
        {
            get { return _user; }
            set { Param_Setter(ref _user, value); }
        }
//}]}

        public SettingsViewModel()
        {
        }

        public async Task InitializeAsync()
        {
            VersionDescription = GetVersionDescription();
//^^
//{[{
            IdentityService.LoggedOut += OnLoggedOut;
            UserDataService.UserDataUpdated += OnUserDataUpdated;
            User = await UserDataService.GetUserAsync();
//}]}
        }
//^^
//{[{

        public void UnregisterEvents()
        {
            IdentityService.LoggedOut -= OnLoggedOut;
            UserDataService.UserDataUpdated -= OnUserDataUpdated;
        }

        private void OnUserDataUpdated(object sender, UserViewModel userData)
        {
            User = userData;
        }

        private async void OnLogout()
        {
            await IdentityService.LogoutAsync();
        }

        private void OnLoggedOut(object sender, EventArgs e)
        {
            UnregisterEvents();
        }
//}]}
    }
}
