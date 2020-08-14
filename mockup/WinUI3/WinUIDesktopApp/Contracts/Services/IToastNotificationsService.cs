using Windows.UI.Notifications;

namespace WinUIDesktopApp.Contracts.Services
{
    public interface IToastNotificationsService
    {
        void ShowToastNotification(ToastNotification toastNotification);

        void ShowToastNotificationSample();
    }
}
