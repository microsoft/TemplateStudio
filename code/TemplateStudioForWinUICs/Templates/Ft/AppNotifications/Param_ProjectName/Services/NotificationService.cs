using Param_ProjectName.Contracts.Services;
using Param_ProjectName.ViewModels;
using Microsoft.Windows.AppNotifications;
using System.Web;

namespace Param_RootNamespace.Notifications;

public class NotificationService : INotificationService
{
    private readonly INavigationService _navigationService;

    public NotificationService(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    ~NotificationService()
    {
        Unregister();
    }

    public void Initialize()
    {
        AppNotificationManager.Default.NotificationInvoked += OnNotificationInvoked;

        AppNotificationManager.Default.Register();
    }

    public void OnNotificationInvoked(AppNotificationManager sender, AppNotificationActivatedEventArgs args)
    {
        // TODO: Handle notification invocations when your app is already running.

        //// Navigate to a specific page based on the notification arguments.
        //if (ParseArguments(args.Argument, "action") == "Settings")
        //{
        //    App.MainWindow.DispatcherQueue.TryEnqueue(() =>
        //    {
        //        _navigationService.NavigateTo(typeof(SettingsViewModel).FullName);
        //    });
        //}
    }

    public bool Show(string payload)
    {
        var appNotification = new AppNotification(payload);

        AppNotificationManager.Default.Show(appNotification);

        return appNotification.Id != 0;
    }

    public string? ParseArguments(string arguments, string parameter)
    {
        return HttpUtility.ParseQueryString(arguments)[parameter];
    }

    public void Unregister()
    {
        AppNotificationManager.Default.Unregister();
    }
}

