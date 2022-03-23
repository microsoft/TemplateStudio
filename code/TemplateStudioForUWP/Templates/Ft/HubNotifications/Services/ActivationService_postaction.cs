//{**
// These code blocks include the HubNotificationsFeatureService Instance in the method `GetActivationHandlers()`
// in the ActivationService of your project and add documentation about how to use the HubNotificationsFeatureService.
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

            // TODO WTS: Configure and enable Azure Notification Hub integration.
            //  1. Go to the HubNotificationsFeatureService class, in the InitializeAsync() method, provide the Hub Name and DefaultListenSharedAccessSignature.
            //  2. Uncomment the following line (an exception will be thrown if it is executed and the above information is not provided).
            // await Singleton<HubNotificationsFeatureService>.Instance.InitializeAsync().ConfigureAwait(false);
//}]}
//{??{
            await Task.CompletedTask;
//}??}
        }


        private IEnumerable<ActivationHandler> GetActivationHandlers()
        {
//{[{
            yield return Singleton<HubNotificationsFeatureService>.Instance;
//}]}
//{--{
            yield break;
//}--}
        }
    }
}
