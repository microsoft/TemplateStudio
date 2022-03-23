//{[{
using Param_RootNamespace.Core.Contracts.Services;
//}]}
namespace Param_RootNamespace.Views
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
        private UserData _user;
//}]}
        private string _versionDescription;
//^^
//{[{
        public UserData User
        {
            get { return _user; }
            set { Set(ref _user, value); }
        }
//}]}
        public string VersionDescription
        {
        }

        public SettingsPage(/*{[{*/IUserDataService userDataService, IIdentityService identityService/*}]}*/)
        {
//^^
//{[{
            _userDataService = userDataService;
            _identityService = identityService;
//}]}
        }

        public void OnNavigatedTo(object parameter)
        {
//^^
//{[{
            _identityService.LoggedOut += OnLoggedOut;
            _userDataService.UserDataUpdated += OnUserDataUpdated;
            User = _userDataService.GetUser();
//}]}
        }

        public void OnNavigatedFrom()
        {
//^^
//{[{
            UnregisterEvents();
//}]}
        }
//{[{

        private void UnregisterEvents()
        {
            _identityService.LoggedOut -= OnLoggedOut;
            _userDataService.UserDataUpdated -= OnUserDataUpdated;
        }
//}]}
        private void OnPrivacyStatementClick(object sender, RoutedEventArgs e)
            => _systemService.OpenInWebBrowser(_appConfig.PrivacyStatement);
//^^
//{[{
        private async void OnLogOut(object sender, RoutedEventArgs e)
        {
            await _identityService.LogoutAsync();
        }

        private void OnUserDataUpdated(object sender, UserData userData)
        {
            User = userData;
        }

        private void OnLoggedOut(object sender, EventArgs e)
        {
            UnregisterEvents();
        }
//}]}
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
