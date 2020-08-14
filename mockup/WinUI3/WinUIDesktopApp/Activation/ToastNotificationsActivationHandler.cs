using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace WinUIDesktopApp.Activation
{
    public class ToastNotificationsActivationHandler : ActivationHandler<ToastNotificationActivatedEventArgs>
    {
        protected override async Task HandleInternalAsync(ToastNotificationActivatedEventArgs args)
        {
            //// TODO WTS: Handle activation from toast notification
            //// More details at https://docs.microsoft.com/windows/uwp/design/shell/tiles-and-notifications/send-local-toast

            await Task.CompletedTask;
        }
    }
}
