using Microsoft.Services.Store.Engagement;
using System;
using Windows.ApplicationModel.Activation;

namespace RootNamespace.Services
{
    public static class StoreNotificationsFeatureService
    {
        public static async void Initialize()
        {
            //See more about sending targeted push notifications to your app's customers
            //https://docs.microsoft.com/windows/uwp/publish/send-push-notifications-to-your-apps-customers

            StoreServicesEngagementManager engagementManager = StoreServicesEngagementManager.GetDefault();
            await engagementManager.RegisterNotificationChannelAsync();
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