using Param_ProjectName.Contracts.Services;
using Param_ProjectName.ViewModels;

using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using Microsoft.Windows.AppNotifications;

namespace Param_RootNamespace.Activation;

public class NotificationActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
{
    private readonly INavigationService _navigationService;
    private readonly INotificationService _notificationService;

    public NotificationActivationHandler(INavigationService navigationService, INotificationService notificationService)
    {
        _navigationService = navigationService;
        _notificationService = notificationService;
    }

    protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
    {
        return AppInstance.GetCurrent().GetActivatedEventArgs()?.Kind == ExtendedActivationKind.AppNotification;
    }

    protected async override Task HandleInternalAsync(LaunchActivatedEventArgs args)
    {
        // TODO: Handle notification activations.

        //// Access the AppNotificationActivatedEventArgs.
        //var activatedEventArgs = (AppNotificationActivatedEventArgs)AppInstance.GetCurrent().GetActivatedEventArgs().Data;

        //// Extract arguments from the notification payload.
        //var action = _notificationService.ParseArguments(activatedEventArgs.Argument, "action");

        //// Take action based on the arguments. Navigating to a specific page is a common scenario.
        //if (action == "ContentGridPage")
        //{
        //    // Navigate to a specific page based on the payload in the notification.
        //    _navigationService.NavigateTo(typeof(ContentGridViewModel).FullName!);
        //}
        await Task.CompletedTask;
    }
}
