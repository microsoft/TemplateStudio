using Param_RootNamespace.Contracts.Services;
using CommunityToolkit.WinUI.Notifications;
using Windows.UI.Notifications;

namespace Param_RootNamespace.Services;

public partial class ToastNotificationsService : IToastNotificationsService
{
    public ToastNotificationsService()
    {
    }

    public void ShowToastNotification(ToastNotification toastNotification)
    {
        ToastNotificationManagerCompat.CreateToastNotifier().Show(toastNotification);
    }
}
