using System;
using Param_RootNamespace.Helpers;

namespace Param_ItemNamespace.Services
{
    internal class ActivationService
    {
        private async Task StartupAsync()
        {
            //TODO UWPTemplates: To use the HubNotificationService especific data related with your Azure Notification Hubs is required.
            //  1. Go to the HubNotificationsFeatureService class, in the InitializeAsync() method, provide the Hub Name and DefaultListenSharedAccessSignature.
            //  2. Uncomment the following line (an exception is thrown if it is executed before the previous information is provided).

            //Singleton<HubNotificationsFeatureService>.Instance.InitializeAsync();
        }

        private IEnumerable<ActivationHandler> GetActivationHandlers()
        {
            yield return Singleton<HubNotificationsFeatureService>.Instance;
//{--{

            yield break;//}--}
        }
    }
}