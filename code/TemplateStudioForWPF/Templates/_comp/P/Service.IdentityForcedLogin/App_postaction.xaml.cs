//{[{
using Microsoft.Extensions.DependencyInjection;
using Prism.Regions;
using System.Linq;
using Unity;
using System.Net.Http;
//}]}
namespace Param_RootNamespace;

public partial class App : PrismApplication
{
//{[{
    private LogInWindow _logInWindow;

    private bool _isInitialized;
//}]}

    protected override async void OnInitialized()
    {
//^^
//{[{
        var userLogged = await TryUserLogin();
        if (!userLogged)
        {
            return;
        }

//}]}
        base.OnInitialized();
//{--{
        await Task.CompletedTask;
//}--}
    }
//{[{

    private async Task<bool> TryUserLogin()
    {
        var userDataService = Container.Resolve<IUserDataService>();
        userDataService.Initialize();

        var identityService = Container.Resolve<IIdentityService>();
        if (!_isInitialized)
        {
            var config = Container.Resolve<AppConfig>();
            identityService.InitializeWithAadAndPersonalMsAccounts(config.IdentityClientId, "http://localhost");
        }

        identityService.LoggedIn += OnLoggedIn;
        identityService.LoggedOut += OnLoggedOut;

        var silentLoginSuccess = await identityService.AcquireTokenSilentAsync();
        if (!silentLoginSuccess || !identityService.IsAuthorized())
        {
            var loginWindow = Application.Current.Windows.OfType<LogInWindow>().FirstOrDefault();
            if (loginWindow != null)
            {
                loginWindow.Activate();
                loginWindow.WindowState = WindowState.Normal;
            }
            else
            {
                ShowLogInWindow();
                _isInitialized = true;
            }

            return false;
        }

        return true;
    }

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
        containerRegistry.GetContainer().RegisterFactory<IHttpClientFactory>(container => GetHttpClientFactory());
        containerRegistry.Register<IIdentityCacheService, IdentityCacheService>();
        containerRegistry.RegisterSingleton<IIdentityService, IdentityService>();
//}]}
        // App Services
//{[{
        containerRegistry.RegisterSingleton<IUserDataService, UserDataService>();
//}]}
    }
//{[{

    private IHttpClientFactory GetHttpClientFactory()
    {
        var services = new ServiceCollection();
        services.AddHttpClient("msgraph", client =>
        {
            client.BaseAddress = new System.Uri("https://graph.microsoft.com/v1.0/");
        });

        return services.BuildServiceProvider().GetRequiredService<IHttpClientFactory>();
    }
//}]}
    private IConfiguration BuildConfiguration()
}
