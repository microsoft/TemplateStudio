//{[{
using Param_RootNamespace.Core.Helpers;
using Param_RootNamespace.Core.Services;
using Param_RootNamespace.Models;
//}]}
namespace Param_RootNamespace.Views
{
    public sealed partial class Param_SettingsPageNamePage : Page, INotifyPropertyChanged
    {
//{[{
        private UserDataService UserDataService => Singleton<UserDataService>.Instance;

        private IdentityService IdentityService => Singleton<IdentityService>.Instance;

//}]}
        private ElementTheme _elementTheme = ThemeSelectorService.Theme;
//{[{
        private UserData _user;
//}]}

//^^
//{[{
        public UserData User
        {
            get { return _user; }
            set { Set(ref _user, value); }
        }
//}]}

        public Param_SettingsPageNamePage()
        {
        }

        private async Task InitializeAsync()
        {
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
        private async void OnLogout(object sender, RoutedEventArgs e)
        {
            await IdentityService.LogoutAsync();
        }
//}]}

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
