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
//}]}
        public async Task ActivateAsync(object activationArgs)
        {
            if (IsInteractive(activationArgs))
            {
                await InitializeAsync();
//{[{
                IdentityService.InitializeWithAadAndPersonalMsAccounts();
                await IdentityService.AcquireTokenSilentAsync();
//}]}
            }
        }
    }
}
