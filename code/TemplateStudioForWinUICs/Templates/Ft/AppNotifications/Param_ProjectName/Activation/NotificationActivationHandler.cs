using System;
using System.Threading.Tasks;

using Param_ProjectName.Contracts.Services;
using Param_ProjectName.Notifications;
using Param_ProjectName.ViewModels;

using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using Microsoft.Windows.AppNotifications;

namespace Param_RootNamespace.Activation;

public class NotificationActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
{
    private readonly INavigationService _navigationService;

    public NotificationActivationHandler(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
    {
        return AppInstance.GetCurrent().GetActivatedEventArgs()?.Kind == ExtendedActivationKind.AppNotification;
    }

    protected async override Task HandleInternalAsync(LaunchActivatedEventArgs args)
    {
        AppActivationArguments activationArgs = AppInstance.GetCurrent().GetActivatedEventArgs();
        var notificationActivatedEventArgs = (AppNotificationActivatedEventArgs)activationArgs.Data;
        var action = NotificationService.ExtractParamFromArgs(notificationActivatedEventArgs.Argument, "action");

        switch (action)
        {
            case "CGridPage":
                _navigationService.NavigateTo(typeof(ContentGridViewModel).FullName);
                break; 

            default:
                _navigationService.NavigateTo(typeof(MainViewModel).FullName);
                break;
        }
        await Task.CompletedTask;
    }
}
