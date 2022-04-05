using System;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Messaging;

using Windows.Networking.PushNotifications;

namespace Param_RootNamespace.Services
{
    // More about adding push notifications to your Windows app at https://docs.microsoft.com/azure/app-service-mobile/app-service-mobile-windows-store-dotnet-get-started-push
    internal class HubNotificationsFeatureService : IHubNotificationsFeatureService
    {
        public async Task InitializeAsync()
        {
            // The code below will throw an exception until it has correct parameters added to it.
            // Once that is done the try/catch can be removed if desired.
            try
            {
                // TODO: Set your Hub Name
                var hubName = string.Empty;

                // TODO: Set your DefaultListenSharedAccessSignature
                var accessSignature = string.Empty;

                var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();

                var hub = new NotificationHub(hubName, accessSignature);
                var result = await hub.RegisterNativeAsync(channel.Uri);
                if (result.RegistrationId != null)
                {
                    // Registration was successful
                }

                // You can also send push notifications from Windows Developer Center targeting your app consumers
                // More details at https://docs.microsoft.com/windows/uwp/publish/send-push-notifications-to-your-apps-customers
            }
            catch (ArgumentNullException)
            {
                // Until a valid accessSignature and hubName are provided this code will throw an ArgumentNullException.
            }
            catch (Exception)
            {
                // TODO: Channel registration call can fail, please handle exceptions as appropriate to your scenario.
            }
        }
    }
}
