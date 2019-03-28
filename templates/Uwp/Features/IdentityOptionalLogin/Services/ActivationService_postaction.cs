//{[{
using Param_RootNamespace.Core.Helpers;
using Param_RootNamespace.Core.Services;
//}]}

namespace Param_RootNamespace.Services
{
    internal class ActivationService
    {
        private object _lastActivationArgs;
//{[{

        private IdentityService IdentityService => Singleton<IdentityService>.Instance;

        private UserDataService UserDataService => Singleton<UserDataService>.Instance;
//}]}
        public async Task ActivateAsync(object activationArgs)
        {
            if (IsInteractive(activationArgs))
            {
                await InitializeAsync();
//{[{
                UserDataService.Initialize();
                IdentityService.InitializeWithAadAndPersonalMsAccounts();
                await IdentityService.AcquireTokenSilentAsync();
//}]}
            }
        }
    }
}
