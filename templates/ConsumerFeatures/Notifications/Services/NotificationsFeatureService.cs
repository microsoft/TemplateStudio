using Microsoft.Services.Store.Engagement;
using Microsoft.WindowsAzure.Messaging;
using System;
using Windows.ApplicationModel.Activation;
using Windows.Data.Xml.Dom;
using Windows.Networking.PushNotifications;
using Windows.UI.Notifications;

namespace RootNamespace.Services
{
    public static class NotificationsFeatureService
    {
        public static async void InitializeNotificationsHub()
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

        public static async void InitializeStoreNotifications()
        {
            //See more about sending targeted push notifications to your app's customers
            //https://docs.microsoft.com/windows/uwp/publish/send-push-notifications-to-your-apps-customers

            StoreServicesEngagementManager engagementManager = StoreServicesEngagementManager.GetDefault();
            await engagementManager.RegisterNotificationChannelAsync();
        }

        public static void ShowToastNotification(string xml)
        {
            XmlDocument toastXml = new XmlDocument();
            toastXml.LoadXml(xml);

            // Generate toast
            var toast = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        public static void ShowToastNotificationSample()
        {
            string xml =
            $@"<toast activationType='foreground'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>Action - text</text>
                            <text>Make sure left button on the toast has the text ""ok"" on it, and the right button has the text ""cancel"" on it.</text>
                        </binding>
                    </visual>
                    <actions>
                        <action
                            content='ok'
                            activationType='foreground'
                            arguments='ok'/>
                        <action
                            content='cancel'
                            activationType='foreground'
                            arguments='cancel'/>
                    </actions>
                </toast>";
            ShowToastNotification(xml);
        }

        public static void HandleNotificationActivation(IActivatedEventArgs args)
        {
            if (args is ToastNotificationActivatedEventArgs)
            {
                var toastActivationArgs = args as ToastNotificationActivatedEventArgs;
                //TODO UWPCommunity: Handle notification parameter activation
            }
        }
    }
}