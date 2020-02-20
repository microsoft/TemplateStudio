//{[{
using Microsoft.Extensions.DependencyInjection;
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