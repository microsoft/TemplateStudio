//{[{
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client.Extensions.Msal;
using Param_RootNamespace.Models;
using Param_RootNamespace.Core.Contracts.Services;
//}]}

namespace Param_RootNamespace.Services
{
    public class ApplicationHostService : IHostedService
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
        public ApplicationHostService(/*{[{*/IIdentityService identityService, IUserDataService userDataService, IOptions<AppConfig> config/*}]}*/)
        {
//^^
//{[{
            _identityService = identityService;
            _userDataService = userDataService;
            _config = config.Value;
//}]}
        }

        public async Task StartAsync(CancellationToken cancellationToken)
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
                _logInWindow = _serviceProvider.GetService(typeof(ILogInWindow)) as ILogInWindow;
                _logInWindow.ShowWindow();
                await StartupAsync();
                return;
            }
//}]}
        }

        public async Task StopAsync(CancellationToken cancellationToken)
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
            _shellWindow = _serviceProvider.GetService(typeof(IShellWindow)) as IShellWindow;
            _navigationService.Initialize(_shellWindow.GetNavigationFrame());
            _shellWindow.ShowWindow();
            _navigationService.NavigateTo(typeof(Param_HomeNameViewModel).FullName);
            _logInWindow.CloseWindow();
        }

        private void OnLoggedOut(object sender, EventArgs e)
        {
            // Show the LogIn Window
            _logInWindow = _serviceProvider.GetService(typeof(ILogInWindow)) as ILogInWindow;
            _logInWindow.ShowWindow();

            // Close the Shell Window and
            _shellWindow.CloseWindow();
            _navigationService.UnsubscribeNavigation();
        }
//}]}
    }
}
