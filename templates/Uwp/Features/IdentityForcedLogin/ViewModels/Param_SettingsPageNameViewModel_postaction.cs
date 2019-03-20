//{[{
using Param_RootNamespace.Core.Helpers;
using Param_RootNamespace.Core.Services;
//}]}
namespace Param_RootNamespace.ViewModels
{
    public class Param_SettingsPageNameViewModel : System.ComponentModel.INotifyPropertyChanged
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

        public Param_SettingsPageNameViewModel()
        {
        }

        public async Task InitializeAsync()
        {
            VersionDescription = GetVersionDescription();
//^^
//{[{
            User = await UserDataService.GetUserFromCacheAsync();
            User = await UserDataService.GetUserFromGraphApiAsync();
            if (User == null)
            {
                User = UserDataService.GetDefaultUserData();
            }
//}]}
            await Task.CompletedTask;
        }

//^^
//{[{
        private async void OnLogout()
        {
            await IdentityService.LogoutAsync();
        }
//}]}
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
