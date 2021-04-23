//{[{
using Param_RootNamespace.Models;
using Param_RootNamespace.Core.Contracts.Services;
//}]}

namespace Param_RootNamespace.Services
{
    public class ApplicationHostService : IApplicationHostService
    {
        private readonly IThemeSelectorService _themeSelectorService;
//{[{
        private readonly IIdentityService _identityService;
        private readonly IUserDataService _userDataService;
        private readonly AppConfig _appConfig;
//}]}
        private IShellWindow _shellWindow;
//{[{
        private ILogInWindow _logInWindow;
//}]}
        public ApplicationHostService(/*{[{*/IIdentityService identityService, IUserDataService userDataService, AppConfig config/*}]}*/)
        {
//^^
//{[{
            _identityService = identityService;
            _userDataService = userDataService;
            _appConfig = config;
//}]}
        }

        public async Task StartAsync()
        {
            await InitializeAsync();
//{[{

            if (!_isInitialized)
            {
                _identityService.InitializeWithAadAndPersonalMsAccounts(_appConfig.IdentityClientId, "http://localhost");
            }

            var silentLoginSuccess = await _identityService.AcquireTokenSilentAsync();
            if (!silentLoginSuccess || !_identityService.IsAuthorized())
            {
                if (!_isInitialized)
                {
                    _logInWindow = SimpleIoc.Default.GetInstance<ILogInWindow>();
                    _logInWindow.ShowWindow();
                    await StartupAsync();
                    _isInitialized = true;
                }

                return;
            }
//}]}
        }

        public async Task StopAsync()
        {
//^^
//{[{
            _identityService.LoggedIn -= OnLoggedIn;
            _identityService.LoggedOut -= OnLoggedOut;
//}]}
        }

        private async Task InitializeAsync()
        {
            if (!_isInitialized)
            {
//^^
//{[{
                _userDataService.Initialize();
                _identityService.LoggedIn += OnLoggedIn;
                _identityService.LoggedOut += OnLoggedOut;
//}]}
                await Task.CompletedTask;
            }
        }
//^^
//{[{

        private async void OnLoggedIn(object sender, EventArgs e)
        {
            await HandleActivationAsync();
            _logInWindow.CloseWindow();
            _logInWindow = null;
        }

        private void OnLoggedOut(object sender, EventArgs e)
        {
            _logInWindow = SimpleIoc.Default.GetInstance<ILogInWindow>(Guid.NewGuid().ToString());
            _logInWindow.ShowWindow();

            _shellWindow.CloseWindow();
            _navigationService.UnsubscribeNavigation();
        }
//}]}
    }
}
