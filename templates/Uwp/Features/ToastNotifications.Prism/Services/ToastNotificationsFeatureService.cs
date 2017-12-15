using Windows.UI.Notifications;

namespace Param_RootNamespace.Services
{
    internal partial class ToastNotificationsFeatureService : IToastNotificationsFeatureService
    {
        public void ShowToastNotification(ToastNotification toastNotification)
        {
            ToastNotificationManager.CreateToastNotifier().Show(toastNotification);
        }
    }
}
