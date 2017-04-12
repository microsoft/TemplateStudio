using System;
using RootNamespace.Helper;

namespace ItemNamespace.Services
{
    internal class ActivationService
    {
        private async Task StartupAsync()
        {
            //TODO UWPTemplates: To use the HubNotificationService especific data related with your Azure Notification Hubs is required.
            //  1. Go to the HubNotificationsService class, in the InitializeAsync() method, provide the Hub Name and DefaultListenSharedAccessSignature.
            //  2. Uncomment the following line (an exception is thrown if it is executed before the previous information is provided).

            //Singleton<HubNotificationsService>.Instance.InitializeAsync();
        }
        private IEnumerable<ActivationHandler> GetActivationHandlers()
        {
            yield return Singleton<HubNotificationsService>.Instance;
        }
    }
}