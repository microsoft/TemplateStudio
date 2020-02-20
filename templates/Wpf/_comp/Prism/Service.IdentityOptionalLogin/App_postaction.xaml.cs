//{[{
using Microsoft.Extensions.DependencyInjection;
//}]}
namespace Param_RootNamespace
{
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
        }

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