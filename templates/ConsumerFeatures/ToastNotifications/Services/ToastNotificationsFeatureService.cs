using Microsoft.Toolkit.Uwp.Notifications;
using Windows.ApplicationModel.Activation;
using Windows.UI.Notifications;

using System.Threading.Tasks;

using RootNamespace.Activation;

namespace RootNamespace.Services
{
    class ToastNotificationsFeatureService : ActivationHandler<ToastNotificationActivatedEventArgs>
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

            await Task.FromResult(true).ConfigureAwait(false);
        }

        public void ShowToastNotificationSample()
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
    }
}