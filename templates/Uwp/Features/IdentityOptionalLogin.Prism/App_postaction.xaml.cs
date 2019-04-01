//{[{
using Param_RootNamespace.Core.Helpers;
using Param_RootNamespace.Core.Services;
//}]}

namespace Param_RootNamespace
{
    public sealed partial class App : PrismUnityApplication
    {
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
//{[{
            var identityService = new IdentityService();
            var microsoftGraphService = new MicrosoftGraphService();
            var userDataService = new UserDataService(identityService, microsoftGraphService);
//}]}
            Container.RegisterInstance<IResourceLoader>(new ResourceLoaderAdapter(new ResourceLoader()));
//{[{
            Container.RegisterInstance<IIdentityService>(identityService);
            Container.RegisterInstance<IMicrosoftGraphService>(microsoftGraphService);
            Container.RegisterInstance<IUserDataService>(userDataService);
//}]}
        }

        protected override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
//{[{
            var userDataService = Container.Resolve<IUserDataService>();
            var identityService = Container.Resolve<IIdentityService>();
            userDataService.Initialize();
            identityService.InitializeWithAadAndPersonalMsAccounts();
//}]}
        }
    }
}
