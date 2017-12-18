using Windows.UI.Notifications;

namespace Param_RootNamespace.Services
{
    internal interface IToastNotificationsFeatureService
    {
        void ShowToastNotification(ToastNotification toastNotification);

        void ShowToastNotificationSample();
    }
}
