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
        private RelayCommand _logInCommand;
        private RelayCommand _logOutCommand;
        private bool _isLoggedIn;
        private bool _isBusy;
        private UserViewModel _user;
//}]}
        public ElementTheme ElementTheme
        {
        }
//^^
//{[{
        public RelayCommand LogInCommand => _logInCommand ?? (_logInCommand = new RelayCommand(OnLogIn, () => !IsBusy));

        public RelayCommand LogOutCommand => _logOutCommand ?? (_logOutCommand = new RelayCommand(OnLogOut, () => !IsBusy));

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set { Param_Setter(ref _isLoggedIn, value); }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                Param_Setter(ref _isBusy, value);
            }
        }

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
            IdentityService.LoggedIn += OnLoggedIn;
            IdentityService.LoggedOut += OnLoggedOut;
            UserDataService.UserDataUpdated += OnUserDataUpdated;
            IsLoggedIn = IdentityService.IsLoggedIn();
            User = await UserDataService.GetUserAsync();
//}]}
        }

//^^
//{[{
        public void UnregisterEvents()
        {
            IdentityService.LoggedIn -= OnLoggedIn;
            IdentityService.LoggedOut -= OnLoggedOut;
            UserDataService.UserDataUpdated -= OnUserDataUpdated;
        }

        private void OnUserDataUpdated(object sender, UserViewModel userData)
        {
            User = userData;
        }

        private async void OnLogIn()
        {
            IsBusy = true;
            var loginResult = await IdentityService.LoginAsync();
            if (loginResult != LoginResultType.Success)
            {
                await AuthenticationHelper.ShowLoginErrorAsync(loginResult);
                IsBusy = false;
            }
        }

        private async void OnLogOut()
        {
            IsBusy = true;
            await IdentityService.LogoutAsync();
        }

        private void OnLoggedIn(object sender, EventArgs e)
        {
            IsLoggedIn = true;
            IsBusy = false;
        }

        private void OnLoggedOut(object sender, EventArgs e)
        {
            User = null;
            IsLoggedIn = false;
            IsBusy = false;
        }
//}]}
    }
}
