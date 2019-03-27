//{[{
using Param_RootNamespace.Models;
using Param_RootNamespace.Core.Helpers;
using Param_RootNamespace.Core.Services;
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
        private bool _isLoggedIn;
        private bool _isBusy;
        private UserData _user;
//}]}
        public ElementTheme ElementTheme
        {
        }
//^^
//{[{
        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set { Set(ref _isLoggedIn, value); }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set { Set(ref _isBusy, value); }
        }

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
            VersionDescription = GetVersionDescription();
//{[{
            IdentityService.LoggedIn += OnLoggedIn;
            IdentityService.LoggedOut += OnLoggeOut;
            await GetUserDataAsync();
//}]}
        }
//^^
//{[{
        private async void OnLogIn(object sender, RoutedEventArgs e)
        {
            IsBusy = true;
            var loginResult = await IdentityService.LoginAsync();
            if (loginResult != LoginResultType.Success)
            {
                await AuthenticationHelper.ShowLoginErrorAsync(loginResult);
                IsBusy = false;
            }
        }

        private async void OnLogOut(object sender, RoutedEventArgs e)
        {
            IsBusy = true;
            await IdentityService.LogoutAsync();
        }

        private async void OnLoggedIn(object sender, EventArgs e)
        {
            await GetUserDataAsync();
            IsBusy = false;
        }

        private async void OnLoggeOut(object sender, EventArgs e)
        {
            await GetUserDataAsync();
            IsBusy = false;
        }

        private async Task GetUserDataAsync()
        {
            IsLoggedIn = IdentityService.IsLoggedIn();
            if (IsLoggedIn)
            {
                User = await UserDataService.GetUserFromCacheAsync();
                User = await UserDataService.GetUserFromGraphApiAsync();
                if (User == null)
                {
                    User = UserDataService.GetDefaultUserData();
                }
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            IdentityService.LoggedIn -= OnLoggedIn;
            IdentityService.LoggedOut -= OnLoggeOut;
        }
//}]}

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
