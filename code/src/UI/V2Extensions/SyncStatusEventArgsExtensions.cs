// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.UI.V2Controls;
using Microsoft.Templates.UI.V2Resources;

namespace Microsoft.Templates.UI.V2Extensions
{
    public static class SyncStatusEventArgsExtensions
    {
        public static Notification GetNotification(this SyncStatusEventArgs args)
        {
            switch (args.Status)
            {
                case SyncStatus.Updating:
                    return Notification.Information(StringRes.NotificationSyncStatus_Updating, Category.TemplatesSync, TimerType.None, false);
                case SyncStatus.Updated:
                    return Notification.Information(StringRes.NotificationSyncStatus_Updated, Category.TemplatesSync, TimerType.Short, true);
                case SyncStatus.Acquiring:
                    return Notification.Information(string.Format(StringRes.NotificationSyncStatus_Acquiring, args.Version, args.Progress), Category.TemplatesSync, TimerType.None, false);
                case SyncStatus.Acquired:
                    return Notification.Information(StringRes.NotificationSyncStatus_Acquired, Category.TemplatesSync, TimerType.None, false);
                case SyncStatus.Preparing:
                    return Notification.Information(string.Format(StringRes.NotificationSyncStatus_Preparing, args.Version), Category.TemplatesSync, TimerType.None, false);
                case SyncStatus.Prepared:
                    return Notification.Information(StringRes.NotificationSyncStatus_Prepared, Category.TemplatesSync, TimerType.Short, true);
                case SyncStatus.NewWizardVersionAvailable:
                    return Notification.Warning(string.Format(StringRes.NotificationSyncStatus_NewWizardVersionAvailable, args.Version), Category.TemplatesSync, TimerType.None);
                case SyncStatus.CheckingForUpdates:
                    return Notification.Information(StringRes.NotificationSyncStatus_CheckingForUpdates, Category.TemplatesSync, TimerType.None, false);
                case SyncStatus.CheckedForUpdates:
                    return Notification.Information(StringRes.NotificationSyncStatus_CheckedForUpdates, Category.TemplatesSync, TimerType.Short, true);
                case SyncStatus.ErrorAcquiring:
                    return Notification.Warning(string.Format(StringRes.NotificationSyncStatus_ErrorAcquiring, args.Version), Category.TemplatesSyncError, TimerType.Large, CategoriesToOverride);
                case SyncStatus.Ready:
                    return null;
                case SyncStatus.None:
                    return null;
                default:
                    return null;
            }
        }

        private static IEnumerable<Category> CategoriesToOverride
        {
            get
            {
                yield return Category.TemplatesSync;
            }
        }
    }
}
