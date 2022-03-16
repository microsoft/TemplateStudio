//{[{
using Param_RootNamespace.Helpers;
using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Helpers;
//}]}
namespace Param_RootNamespace.ViewModels
{
    {
//^^
//{[{
        private readonly IUserDataService _userDataService;
        private readonly IIdentityService _identityService;
//}]}
        private readonly IThemeSelectorService _themeSelectorService;
//^^
//{[{
        private bool _isBusy;
        private bool _isLoggedIn;
        private UserViewModel _user;
//}]}
        private ICommand _setThemeCommand;
        private ICommand _privacyStatementCommand;
//{[{
        private System.Windows.Input.ICommand _logInCommand;
        private System.Windows.Input.ICommand _logOutCommand;
//}]}
        public string VersionDescription
        {
        }
//^^
//{[{
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                Param_Setter(ref _isBusy, value);
                LogInCommand.Param_CanExecuteChangedMethodName();
                LogOutCommand.Param_CanExecuteChangedMethodName();
            }
        }

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set { Param_Setter(ref _isLoggedIn, value); }
        }

        public UserViewModel User
        {
            get { return _user; }
            set { Param_Setter(ref _user, value); }
        }
//}]}
        public ICommand SetThemeCommand => _setThemeCommand ?? (_setThemeCommand = new System.Windows.Input.ICommand<string>(OnSetTheme));

        public ICommand PrivacyStatementCommand => _privacyStatementCommand ?? (_privacyStatementCommand = new System.Windows.Input.ICommand(OnPrivacyStatement));
//{[{

        public System.Windows.Input.ICommand LogInCommand => _logInCommand ?? (_logInCommand = new System.Windows.Input.ICommand(OnLogIn, () => !IsBusy));

        public System.Windows.Input.ICommand LogOutCommand => _logOutCommand ?? (_logOutCommand = new System.Windows.Input.ICommand(OnLogOut, () => !IsBusy));
//}]}
        public SettingsViewModel(/*{[{*/IUserDataService userDataService, IIdentityService identityService/*}]}*/)
        {
//^^
//{[{
            _userDataService = userDataService;
            _identityService = identityService;
//}]}
        }

        public void OnNavigatedTo(Param_OnNavigatedToParams)
        {
//^^
//{[{
            _identityService.LoggedIn += OnLoggedIn;
            _identityService.LoggedOut += OnLoggedOut;
            IsLoggedIn = _identityService.IsLoggedIn();
            _userDataService.UserDataUpdated += OnUserDataUpdated;
            User = _userDataService.GetUser();
//}]}
        }

        public void OnNavigatedFrom(Param_OnNavigatedFromParams)
        {
//^^
//{[{
            UnregisterEvents();
//}]}
        }
//{[{

        private void UnregisterEvents()
        {
            _identityService.LoggedIn -= OnLoggedIn;
            _identityService.LoggedOut -= OnLoggedOut;
            _userDataService.UserDataUpdated -= OnUserDataUpdated;
        }
//}]}
        private void OnPrivacyStatement()
            => _systemService.OpenInWebBrowser(_appConfig.PrivacyStatement);
//^^
//{[{

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
            await _identityService.LogoutAsync();
        }

        private void OnUserDataUpdated(object sender, UserViewModel userData)
        {
            User = userData;
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
