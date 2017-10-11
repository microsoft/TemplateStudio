using Windows.UI.Notifications;

namespace Param_RootNamespace.Services
{
    internal interface IToastNotificationsService
    {
        void ShowToastNotification(ToastNotification toastNotification);
        void ShowToastNotificationSample();
    }
}