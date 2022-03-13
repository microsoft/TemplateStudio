using System;
using Windows.UI.Notifications;

namespace Param_RootNamespace.Services
{
    internal partial class ToastNotificationsFeatureService : IToastNotificationsFeatureService
    {
        public void ShowToastNotification(ToastNotification toastNotification)
        {
            try
            {
                ToastNotificationManager.CreateToastNotifier().Show(toastNotification);
            }
            catch (Exception)
            {
                // TODO WTS: Adding ToastNotification can fail in rare conditions, please handle exceptions as appropriate to your scenario.
            }
        }
    }
}
