//{[{
using Param_RootNamespace.Core.Helpers;
using Param_RootNamespace.Core.Services;
//}]}

namespace Param_RootNamespace
{
    public sealed partial class App : Application
    {
//{[{
        private IdentityService IdentityService => Singleton<IdentityService>.Instance;
//}]}

        public App()
        {
            _activationService = new Lazy<ActivationService>(CreateActivationService);
//^^
//{[{
            IdentityService.LoggedOut += OnLoggedOut;
//}]}
        }

//^^
//{[{
        private async void OnLoggedOut(object sender, EventArgs e)
        {
            await ActivationService.RedirectLoginPageAsync();
        }
//}]}
    }
}
