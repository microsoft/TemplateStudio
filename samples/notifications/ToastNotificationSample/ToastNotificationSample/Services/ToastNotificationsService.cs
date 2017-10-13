using System;
using System.Threading.Tasks;

using ToastNotificationSample.Activation;

using Windows.ApplicationModel.Activation;
using Windows.UI.Notifications;

namespace ToastNotificationSample.Services
{
    internal partial class ToastNotificationsService : ActivationHandler<ToastNotificationActivatedEventArgs>
    {
        public void ShowToastNotification(ToastNotification toastNotification)
        {
            ToastNotificationManager.CreateToastNotifier().Show(toastNotification);
        }

        protected override async Task HandleInternalAsync(ToastNotificationActivatedEventArgs args)
        {
            NavigationService.Navigate<Views.ActivatedFromToastPage>(args);
            await Task.CompletedTask;
        }
    }
}
