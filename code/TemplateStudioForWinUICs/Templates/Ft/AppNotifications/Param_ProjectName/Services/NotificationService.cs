// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Param_ProjectName.Contracts.Services;
using Microsoft.Windows.AppNotifications;

namespace Param_RootNamespace.Notifications;
internal class NotificationService : INotificationService
{
    ~NotificationService()
    {
        Unregister();
    }
    public void Initialize()
    {
        var notificationManager = AppNotificationManager.Default;
        notificationManager.Register();
        ToastExample.SendToast(); // notification on startup
    }
    public void Unregister()
    {
        AppNotificationManager.Default.Unregister();
    }

}

