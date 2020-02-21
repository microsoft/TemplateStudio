//{[{
using Microsoft.Extensions.DependencyInjection;
using Prism.Regions;
//}]}
namespace Param_RootNamespace
{
    public partial class App : PrismApplication
    {
//{[{
        private LogInWindow _logInWindow;
//}]}

        protected override async void OnInitialized()
        {
//^^
//{[{
            var userDataService = Container.Resolve<IUserDataService>();
            userDataService.Initialize();

            var config = Container.Resolve<AppConfig>();
            var identityService = Container.Resolve<IIdentityService>();
            identityService.InitializeWithAadAndPersonalMsAccounts(config.IdentityClientId, "http://localhost");
            identityService.LoggedIn += OnLoggedIn;
            identityService.LoggedOut += OnLoggedOut;

            var silentLoginSuccess = await identityService.AcquireTokenSilentAsync();
            if (!silentLoginSuccess || !identityService.IsAuthorized())
            {
                ShowLogInWindow();
                return;
            }

//}]}
            base.OnInitialized();
        }
//{[{

        private void OnLoggedIn(object sender, EventArgs e)
        {
            if (!(Application.Current.MainWindow is ShellWindow))
            {
                Application.Current.MainWindow = CreateShell();
                RegionManager.UpdateRegions();
            }

            Application.Current.MainWindow.Show();
            _logInWindow.Close();
        }

        private void OnLoggedOut(object sender, EventArgs e)
        {
            ShowLogInWindow();
            Application.Current.MainWindow.Close();
        }

        private void ShowLogInWindow()
        {
            _logInWindow = Container.Resolve<LogInWindow>();
            _logInWindow.Show();
        }
//}]}
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Core Services
//{[{
            containerRegistry.Register<IMicrosoftGraphService, MicrosoftGraphService>();

            PrismContainerExtension.Create(Container.GetContainer());
            PrismContainerExtension.Current.RegisterServices(s =>
            {
                s.AddHttpClient("msgraph", client =>
                {
                    client.BaseAddress = new System.Uri("https://graph.microsoft.com/v1.0/");
                });
            });

            containerRegistry.Register<IIdentityCacheService, IdentityCacheService>();
            containerRegistry.RegisterSingleton<IIdentityService, IdentityService>();
//}]}
            // App Services
//{[{
            containerRegistry.RegisterSingleton<IUserDataService, UserDataService>();
//}]}
        }
    }
}