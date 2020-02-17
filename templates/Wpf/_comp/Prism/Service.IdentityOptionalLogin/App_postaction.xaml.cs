//{[{
using Microsoft.Extensions.DependencyInjection;
//}]}
namespace Param_RootNamespace
{
    public partial class App : PrismApplication
    {
        protected async override void InitializeShell(Window shell)
        {
//^^
//{[{
            var userDataService = Container.Resolve<IUserDataService>();
            userDataService.Initialize();
            var identityService = Container.Resolve<IIdentityService>();
            identityService.InitializeWithAadAndPersonalMsAccounts("c804973e-54f9-44e0-b49a-05c935435eac", "http://localhost");
            await identityService.AcquireTokenSilentAsync();
//}]}
        }

        protected async override void RegisterTypes(IContainerRegistry containerRegistry)
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