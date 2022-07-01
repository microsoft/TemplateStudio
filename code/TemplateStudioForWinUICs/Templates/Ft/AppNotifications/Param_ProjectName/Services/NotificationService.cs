// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

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
        // TODO: Handle notification invocations when your app is in the foreground.

        //// Extract arguments from the notification payload.
        //var action = ParseArguments(args.Argument, "action");

        //// Take action based on the arguments. Navigating to a specific page is a common scenario.
        //if (action == "ContentGridPage")
        //{
        //    // Make sure you're on the UI thread before navigating.
        //    if (App.MainWindow.DispatcherQueue.HasThreadAccess)
        //    {
        //        // Navigate to a specific page based on the payload in the notification.
        //        _navigationService.NavigateTo(typeof(ContentGridViewModel).FullName!);
        //    }
        //    else
        //    {
        //        App.MainWindow.DispatcherQueue.TryEnqueue(() =>
        //        {
        //             // Navigate to a specific page based on the payload in the notification.
        //            _navigationService.NavigateTo(typeof(ContentGridViewModel).FullName!);
        //        });
        //    }
        //}
    }

    public bool Show(string payload)
    {
        var appNotification = new AppNotification(payload);
        AppNotificationManager.Default.Show(appNotification);
        return appNotification.Id != 0;
    }

    public string? ParseArguments(string args, string parameter)
    {
        return HttpUtility.ParseQueryString(args)[parameter];
    }

    public void Unregister()
    {
        AppNotificationManager.Default.Unregister();
    }

}

