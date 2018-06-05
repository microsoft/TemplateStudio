using System;
using Windows.ApplicationModel.Activation;
using Windows.UI.Notifications;

using System.Threading.Tasks;

using Param_RootNamespace.Activation;

namespace Param_RootNamespace.Services
{
    internal partial class ToastNotificationsFeatureService : ActivationHandler<ToastNotificationActivatedEventArgs>
    {
        public void ShowToastNotification(ToastNotification toastNotification)
        {
            ToastNotificationManager.CreateToastNotifier().Show(toastNotification);
        }

        protected override async Task HandleInternalAsync(ToastNotificationActivatedEventArgs args)
        {
            //// TODO WTS: Handle activation from toast notification
            //// More details at https://docs.microsoft.com/windows/uwp/design/shell/tiles-and-notifications/send-local-toast

            await Task.CompletedTask;
        }
    }
}
