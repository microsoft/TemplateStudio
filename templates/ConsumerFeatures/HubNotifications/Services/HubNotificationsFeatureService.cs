using Microsoft.WindowsAzure.Messaging;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Networking.PushNotifications;

using RootNamespace.Activation;

namespace RootNamespace.Services
{
    internal class HubNotificationsFeatureService : ActivationHandler<ToastNotificationActivatedEventArgs>
    {
        public async void InitializeAsync()
        {
            //See more about adding push notifications to your Windows app
            //https://docs.microsoft.com/azure/app-service-mobile/app-service-mobile-windows-store-dotnet-get-started-push            

            //Use your Hub Name here
            var hubName = "";
            //Use your DefaultListenSharedAccessSignature here
            var accessSignature = "";

            var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();

            var hub = new NotificationHub(hubName, accessSignature);
            var result = await hub.RegisterNativeAsync(channel.Uri);
            if (result.RegistrationId != null)
            {
                //RegistrationID let you know it was successful
            }

            //You can also send push notifications from Windows Developer Center targeting your app consumers
            //https://docs.microsoft.com/windows/uwp/publish/send-push-notifications-to-your-apps-customers
        }

        protected override async Task HandleInternalAsync(ToastNotificationActivatedEventArgs args)
        {
            //TODO UWPTemplates: Handle activation from toast notification,
            //for more info handling activation see
            //https://blogs.msdn.microsoft.com/tiles_and_toasts/2015/07/08/quickstart-sending-a-local-toast-notification-and-handling-activations-from-it-windows-10/

            await Task.CompletedTask;
        }
    }
}