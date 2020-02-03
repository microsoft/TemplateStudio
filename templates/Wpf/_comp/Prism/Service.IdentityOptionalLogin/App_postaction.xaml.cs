//{[{
using Microsoft.Identity.Client.Extensions.Msal;
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
            await identityService.AcquireTokenSilentAsync();
//}]}
        }

        protected async override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Core Services
//^^
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
//^^
//{[{
            containerRegistry.RegisterSingleton<IUserDataService, UserDataService>();
//}]}
            // Views
        }
    }
}