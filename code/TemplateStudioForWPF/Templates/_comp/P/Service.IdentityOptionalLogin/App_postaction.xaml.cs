//{[{
using Microsoft.Extensions.DependencyInjection;
using Unity;
using System.Net.Http;
//}]}
namespace Param_RootNamespace;

public partial class App : PrismApplication
{
    protected override async void OnInitialized()
    {
//^^
//{[{
        var userDataService = Container.Resolve<IUserDataService>();
        userDataService.Initialize();

        var config = Container.Resolve<AppConfig>();
        var identityService = Container.Resolve<IIdentityService>();
        identityService.InitializeWithAadAndPersonalMsAccounts(config.IdentityClientId, "http://localhost");

        await identityService.AcquireTokenSilentAsync();

//}]}
        base.OnInitialized();
//{--{
        await Task.CompletedTask;
//}--}
    }

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
