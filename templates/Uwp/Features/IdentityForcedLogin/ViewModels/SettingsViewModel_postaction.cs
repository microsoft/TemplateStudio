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
            set { Set(ref _user, value); }
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
            IdentityService.LoggedOut += OnLoggeOut;
            UserDataService.UserDataUpdated += OnUserDataUpdated;
            User = await UserDataService.GetUserAsync();
//}]}
        }
//^^
//{[{

        public void UnregisterEvents()
        {
            IdentityService.LoggedOut -= OnLoggeOut;
            UserDataService.UserDataUpdated -= OnUserDataUpdated;
        }

        private void OnUserDataUpdated(object sender, UserViewModel user)
        {
            User = user;
        }

        private async void OnLogout()
        {
            await IdentityService.LogoutAsync();
        }

        private void OnLoggeOut(object sender, EventArgs e)
        {
            UnregisterEvents();
        }
//}]}
    }
}
