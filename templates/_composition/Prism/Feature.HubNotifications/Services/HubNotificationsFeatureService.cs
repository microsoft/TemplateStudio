using System;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Messaging;

using Windows.Networking.PushNotifications;

namespace Param_RootNamespace.Services
{
    internal class HubNotificationsFeatureService : IHubNotificationsFeatureService
    {
        public async Task InitializeAsync()
        {
            // The code below will throw an exception until it had correct parameters added to it.
            // Once that is done the try/catch can be removed if desired.
            try
            {
                //// See more about adding push notifications to your Windows app at
                //// https://docs.microsoft.com/azure/app-service-mobile/app-service-mobile-windows-store-dotnet-get-started-push

                // Specify your Hub Name here
                var hubName = string.Empty;

                // Specify your DefaultListenSharedAccessSignature here
                var accessSignature = string.Empty;

                var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();

                var hub = new NotificationHub(hubName, accessSignature);
                var result = await hub.RegisterNativeAsync(channel.Uri);
                if (result.RegistrationId != null)
                {
                    // Registration was successful
                }

                // You can also send push notifications from Windows Developer Center targeting your app consumers
                // Documentation: https://docs.microsoft.com/windows/uwp/publish/send-push-notifications-to-your-apps-customers
            }
            catch (Exception)
            {
                // Until a valid accessSignature and hubName are provided this code will throw an ArgumentNull Exception.
            }
        }
    }
}
