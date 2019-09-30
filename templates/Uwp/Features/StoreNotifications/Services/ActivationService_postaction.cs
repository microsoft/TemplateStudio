//{**
// These code blocks include the StoreNotificationsFeatureService Instance in the method `GetActivationHandlers()`
// and initializes it in the method `InitializeAsync()` in the ActivationService of your project.
//**}

using System;
//{[{
using Param_RootNamespace.Core.Helpers;
//}]}

namespace Param_RootNamespace.Services
{
    internal class ActivationService
    {
        private async Task StartupAsync()
        {
//^^
//{[{
            await Singleton<StoreNotificationsFeatureService>.Instance.InitializeAsync().ConfigureAwait(false);
//}]}
//{??{
            await Task.CompletedTask;
//}??}
        }

        private IEnumerable<ActivationHandler> GetActivationHandlers()
        {
//{[{
            yield return Singleton<StoreNotificationsFeatureService>.Instance;
//}]}
//{--{
            yield break;
//}--}
        }
    }
}
