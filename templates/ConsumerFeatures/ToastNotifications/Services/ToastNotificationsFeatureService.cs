using Microsoft.Toolkit.Uwp.Notifications;
using Windows.ApplicationModel.Activation;
using Windows.UI.Notifications;

using System.Threading.Tasks;

using RootNamespace.Activation;

namespace RootNamespace.Services
{
    internal class ToastNotificationsFeatureService : ActivationHandler<ToastNotificationActivatedEventArgs>
    {
        public void ShowToastNotification(ToastNotification toastNotification)
        {
            ToastNotificationManager.CreateToastNotifier().Show(toastNotification);
        }

        protected override async Task HandleInternalAsync(ToastNotificationActivatedEventArgs args)
        {
            //TODO UWPTemplates: Handle activation from toast notification,
            //for more info handling activation see
            //https://blogs.msdn.microsoft.com/tiles_and_toasts/2015/07/08/quickstart-sending-a-local-toast-notification-and-handling-activations-from-it-windows-10/

            await Task.CompletedTask;
        }

        public void ShowToastNotificationSample()
        {            
            // Create the toast content
            var content = new ToastContent()
            {
                //TODO UWPTemplates: Check this documentation to know more about the Launch property
                //https://developer.microsoft.com/windows/uwp-community-toolkit/api/microsoft_toolkit_uwp_notifications_toastcontent                
                Launch = "ToastContentActivationParams",

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
                        //TODO UWPTemplates: Check this documentation to know more about Toast Buttons
                        //https://developer.microsoft.com/windows/uwp-community-toolkit/api/microsoft_toolkit_uwp_notifications_toastbutton
                        new ToastButton("Ok", "ToastButtonActivationArguments")
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
                //TODO UWPTemplates: Gets or sets the unique identifier of this notification within the notification Group
                //https://docs.microsoft.com/uwp/api/windows.ui.notifications.toastnotification
                Tag = "ToastNotificationTag"
            };

            // And show the toast
            ShowToastNotification(toast);
        }
    }
}