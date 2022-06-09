// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.SharedResources;
using Microsoft.Templates.UI.Controls;

namespace Microsoft.Templates.UI.Extensions
{
    public static class SyncStatusEventArgsExtensions
    {
        public static Notification GetNotification(this SyncStatusEventArgs args)
        {
            switch (args.Status)
            {
                case SyncStatus.Updating:
                    return Notification.Information(Resources.NotificationSyncStatus_Updating, Category.TemplatesSync, TimerType.None, false);
                case SyncStatus.Updated:
                    return Notification.Information(Resources.NotificationSyncStatus_Updated, Category.TemplatesSync, TimerType.Short, true);
                case SyncStatus.CheckingForUpdates:
                    return Notification.Information(Resources.NotificationSyncStatus_CheckingForUpdates, Category.TemplatesSync, TimerType.None, false);
                case SyncStatus.Acquiring:
                    return Notification.Information(string.Format(Resources.NotificationSyncStatus_Acquiring, args.Version, args.Progress), Category.TemplatesSync, TimerType.None, false);
                case SyncStatus.Preparing:
                    return Notification.Information(string.Format(Resources.NotificationSyncStatus_Preparing, args.Version, args.Progress), Category.TemplatesSync, TimerType.None, false);
                case SyncStatus.NewWizardVersionAvailable:
                    return Notification.Warning(string.Format(Resources.NotificationSyncStatus_NewWizardVersionAvailable, args.Version), Category.TemplatesSync, TimerType.None);
                case SyncStatus.ErrorAcquiring:
                    return Notification.Warning(string.Format(Resources.NotificationSyncStatus_ErrorAcquiring, args.Version), Category.TemplatesSyncError, TimerType.Large, CategoriesToOverride);
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
