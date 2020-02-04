//{[{
using Microsoft.Identity.Client.Extensions.Msal;
//}]}
namespace Param_RootNamespace
{
    public partial class App : PrismApplication
    {
//{[{
        private LogInWindow _logInWindow;
//}]}

        protected async override void InitializeShell(Window shell)
        {
//^^
//{[{
            var userDataService = Container.Resolve<IUserDataService>();
            userDataService.Initialize();
            var identityService = Container.Resolve<IIdentityService>();
            identityService.LoggedIn += OnLoggedIn;
            identityService.LoggedOut += OnLoggedOut;
            var silentLoginSuccess = await identityService.AcquireTokenSilentAsync();
            if (!silentLoginSuccess || !identityService.IsAuthorized())
            {
                ShowLogInWindow();
            }
//}]}
        }
//{[{

        private void OnLoggedIn(object sender, EventArgs e)
        {
            Application.Current.MainWindow.Show();
            _logInWindow.Close();
        }

        private void OnLoggedOut(object sender, EventArgs e)
        {
            Application.Current.MainWindow.Hide();
            ShowLogInWindow();
        }

        private void ShowLogInWindow()
        {
            _logInWindow = Container.Resolve<LogInWindow>();
            _logInWindow.Closed += OnLogInWindowClosed;
            _logInWindow.ShowDialog();
        }

        private void OnLogInWindowClosed(object sender, EventArgs e)
        {
            if (sender is Window window)
            {
                window.Closed -= OnLogInWindowClosed;
                var identityService = Container.Resolve<IIdentityService>();
                if (!identityService.IsLoggedIn())
                {
                    Application.Current.Shutdown();
                }
            }
        }
//}]}
        protected async override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Core Services
//{[{
            containerRegistry.Register<IMicrosoftGraphService, MicrosoftGraphService>();

            // https://aka.ms/msal-net-token-cache-serialization
            var identityService = new IdentityService();
            var storageCreationProperties = new StorageCreationPropertiesBuilder(".msalcache.dat", "MSAL_CACHE", "31f2256a-e9aa-4626-be94-21c17add8fd9").Build();
            var cacheHelper = await MsalCacheHelper.CreateAsync(storageCreationProperties).ConfigureAwait(false);
            identityService.InitializeWithAadAndPersonalMsAccounts("31f2256a-e9aa-4626-be94-21c17add8fd9", "http://localhost", cacheHelper);
            containerRegistry.RegisterInstance<IIdentityService>(identityService);
//}]}
            // App Services
//{[{
            containerRegistry.RegisterSingleton<IUserDataService, UserDataService>();
//}]}
        }
    }
}