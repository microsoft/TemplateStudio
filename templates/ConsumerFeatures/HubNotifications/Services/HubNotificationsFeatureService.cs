using Microsoft.WindowsAzure.Messaging;
using System;
using Windows.ApplicationModel.Activation;
using Windows.Networking.PushNotifications;

namespace RootNamespace.Services
{
    public static class HubNotificationsFeatureService
    {
        public static async void Initialize()
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
        }        

        public static void HandleNotificationActivation(IActivatedEventArgs args)
        {
            if (args is ToastNotificationActivatedEventArgs)
            {                
                var toastArgs = args as ToastNotificationActivatedEventArgs;
                var arguments = toastArgs.Argument; 
                
                //TODO UWPTemplates: Handle activation from toast notification,  
                //for more info handling activation see  
                //https://blogs.msdn.microsoft.com/tiles_and_toasts/2015/07/08/quickstart-sending-a-local-toast-notification-and-handling-activations-from-it-windows-10/ 
            }
        }
    }
}