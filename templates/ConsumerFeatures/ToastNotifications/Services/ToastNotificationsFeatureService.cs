using Microsoft.Toolkit.Uwp.Notifications;
using Windows.ApplicationModel.Activation;
using Windows.UI.Notifications;

namespace RootNamespace.Services
{
    public static class ToastNotificationsFeatureService
    {
        public static void ShowToastNotification(ToastNotification toastNotification)
        {
            ToastNotificationManager.CreateToastNotifier().Show(toastNotification);
        }

        public static void ShowToastNotificationSample()
        {
            // Create the toast content
            var content = new ToastContent()
            {
                Launch = "action=view&id=5",

                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = "Action - text"
                            },

                            new AdaptiveText()
                            {
                                Text = @"Make sure the left button on the toast has the text ""Ok"" on it, and the right button has the text ""Cancel"" on it."
                            }
                        }
                    }
                },

                Actions = new ToastActionsCustom()
                {
                    Buttons =
                    {
                        new ToastButton("Ok", "action=ok&id=5")
                        {
                            ActivationType = ToastActivationType.Foreground
                        },

                        new ToastButtonDismiss("Cancel")
                    }
                }
            };

            // Create the toast
            var toast = new ToastNotification(content.GetXml())
            {
                // Assign a tag (and optionally a group) so you can remove this notification in the future
                Tag = "toast5"
            };

            // And show the toast
            ShowToastNotification(toast);
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