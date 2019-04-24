//{[{
using Param_RootNamespace.Core.Helpers;
using Param_RootNamespace.Core.Services;
using Param_RootNamespace.Models;
//}]}
namespace Param_RootNamespace.Views
{
    public sealed partial class SettingsPage : Page, INotifyPropertyChanged
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

        public SettingsPage()
        {
        }

        private async Task InitializeAsync()
        {
//^^
//{[{
            User = await UserDataService.GetUserAsync();
//}]}
            await Task.CompletedTask;
        }
//^^
//{[{
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            UnregisterEvents();
        }

        public void UnregisterEvents()
        {
            IdentityService.LoggedOut -= OnLoggedOut;
            UserDataService.UserDataUpdated -= OnUserDataUpdated;
        }

        private void OnUserDataUpdated(object sender, UserData user)
        {
            User = user;
        }

        private async void OnLogout(object sender, RoutedEventArgs e)
        {
            await IdentityService.LogoutAsync();
        }

        private void OnLoggedOut(object sender, EventArgs e)
        {
            UnregisterEvents();
        }
//}]}

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
