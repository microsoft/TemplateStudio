//{[{
using System.Collections.Generic;
using Prism.Windows.Navigation;
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
        private DelegateCommand _logInCommand;
        private DelegateCommand _logOutCommand;
        private bool _isLoggedIn;
        private bool _isBusy;
        private UserViewModel _user;
//}]}
        public ElementTheme ElementTheme
        {
        }
//^^
//{[{
        public DelegateCommand LogInCommand => _logInCommand ?? (_logInCommand = new DelegateCommand(OnLogIn, () => !IsBusy));

        public DelegateCommand LogOutCommand => _logOutCommand ?? (_logOutCommand = new DelegateCommand(OnLogOut, () => !IsBusy));

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set { SetProperty(ref _isLoggedIn, value); }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                SetProperty(ref _isBusy, value);
                LogInCommand.RaiseCanExecuteChanged();
                LogOutCommand.RaiseCanExecuteChanged();
            }
        }

        public UserViewModel User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }
//}]}

        public SettingsViewModel(/*{[{*/IIdentityService identityService, IUserDataService userDataService/*}]}*/)
        {
//^^
//{[{
            _identityService = identityService;
            _userDataService = userDataService;
//}]}
        }


        public async Task InitializeAsync()
        {
//^^
//{[{
            _identityService.LoggedIn += OnLoggedIn;
            _identityService.LoggedOut += OnLoggedOut;
            _userDataService.UserDataUpdated += OnUserDataUpdated;
            IsLoggedIn = _identityService.IsLoggedIn();
            User = await _userDataService.GetUserAsync();
//}]}
        }

//^^
//{[{
        public override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
        {
            base.OnNavigatingFrom(e, viewModelState, suspending);
            _identityService.LoggedIn -= OnLoggedIn;
            _identityService.LoggedOut -= OnLoggedOut;
            _userDataService.UserDataUpdated -= OnUserDataUpdated;
        }

        private void OnUserDataUpdated(object sender, UserViewModel userData)
        {
            User = userData;
        }

        private async void OnLogIn()
        {
            IsBusy = true;
            var loginResult = await _identityService.LoginAsync();
            if (loginResult != LoginResultType.Success)
            {
                await AuthenticationHelper.ShowLoginErrorAsync(loginResult);
                IsBusy = false;
            }
        }

        private async void OnLogOut()
        {
            IsBusy = true;
            await _identityService.LogoutAsync();
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
