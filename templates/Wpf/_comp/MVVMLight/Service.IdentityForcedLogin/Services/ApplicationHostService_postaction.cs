//{[{
using Param_RootNamespace.Models;
using Param_RootNamespace.Core.Contracts.Services;
using Microsoft.Identity.Client.Extensions.Msal;
//}]}

namespace Param_RootNamespace.Services
{
    public class ApplicationHostService : IApplicationHostService
    {
        private readonly IThemeSelectorService _themeSelectorService;
//{[{
        private readonly IIdentityService _identityService;
        private readonly IUserDataService _userDataService;
        private readonly AppConfig _config;
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
            _config = config;
//}]}
        }

        public async Task StartAsync()
        {
            await InitializeAsync();
//{[{

            // https://aka.ms/msal-net-token-cache-serialization
            var storageCreationProperties = new StorageCreationPropertiesBuilder(_config.IdentityCacheFileName, _config.IdentityCacheDirectoryName, _config.IdentityClientId).Build();
            var cacheHelper = await MsalCacheHelper.CreateAsync(storageCreationProperties).ConfigureAwait(false);
            _identityService.InitializeWithAadAndPersonalMsAccounts(_config.IdentityClientId, "http://localhost", cacheHelper);
            var silentLoginSuccess = await _identityService.AcquireTokenSilentAsync();
            if (!silentLoginSuccess || !_identityService.IsAuthorized())
            {
                _logInWindow = SimpleIoc.Default.GetInstance<ILogInWindow>();
                _logInWindow.ShowWindow();
                await StartupAsync();
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
//^^
//{[{
            _userDataService.Initialize();
            _identityService.LoggedIn += OnLoggedIn;
            _identityService.LoggedOut += OnLoggedOut;
//}]}
        }
//^^
//{[{

        private void OnLoggedIn(object sender, EventArgs e)
        {
            _shellWindow = SimpleIoc.Default.GetInstance<IShellWindow>(Guid.NewGuid().ToString());
            _navigationService.Initialize(_shellWindow.GetNavigationFrame());
            _shellWindow.ShowWindow();
            _navigationService.NavigateTo(typeof(MainViewModel).FullName);
            _logInWindow.CloseWindow();
            _logInWindow = null;
        }

        private void OnLoggedOut(object sender, EventArgs e)
        {
            // Show the LogIn Window
            _logInWindow = SimpleIoc.Default.GetInstance<ILogInWindow>(Guid.NewGuid().ToString());
            _logInWindow.ShowWindow();

            // Close the Shell Window and
            _shellWindow.CloseWindow();
            _navigationService.UnsubscribeNavigation();
        }
//}]}
    }
}
