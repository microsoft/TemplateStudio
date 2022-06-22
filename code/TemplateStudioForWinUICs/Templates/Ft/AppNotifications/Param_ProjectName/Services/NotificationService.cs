// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using Param_ProjectName.Activation;
using Param_ProjectName.Contracts.Services;
using Param_ProjectName.Services;
using Param_ProjectName.ViewModels;
using Microsoft.UI.Dispatching;
using Microsoft.Windows.AppLifecycle;
using Microsoft.Windows.AppNotifications;

namespace Param_RootNamespace.Notifications;
public class NotificationService : INotificationService
{
    ~NotificationService()
    {
        Unregister();
    }
    public void Initialize()
    {
        var notificationManager = AppNotificationManager.Default;
        notificationManager.NotificationInvoked += OnNotificationInvoked;
        notificationManager.Register();
        ToastExample.SendToast(); // notification on startup
    }
    public static void OnNotificationInvoked(AppNotificationManager sender, AppNotificationActivatedEventArgs args)
    {
        INavigationService navigationService = App.GetService<INavigationService>();
        var action = ExtractParamFromArgs(args.Argument, "action");
        if (action == "CGridPage")
        {
            if (App.MainWindow.DispatcherQueue.HasThreadAccess)
            {
                navigationService.NavigateTo(typeof(ContentGridViewModel).FullName);
            }
            else
            {
                App.MainWindow.DispatcherQueue.TryEnqueue(() =>
                {
                    navigationService.NavigateTo(typeof(ContentGridViewModel).FullName);
                });

            }
        }
    }

    public static string ExtractParamFromArgs(string args, string paramName)
    {
        var tag = paramName;
        tag += "=";

        var tagStart = args.IndexOf(tag);
        if (tagStart == -1)
        {
            return null;
        }

        var paramStart = tagStart + tag.Length;

        var paramEnd = args.IndexOf("&", paramStart);
        if (paramEnd == -1)
        {
            paramEnd = args.Length;
        }

        return args.Substring(paramStart, paramEnd - paramStart);
    }
    public void Unregister()
    {
        AppNotificationManager.Default.Unregister();
    }

}

