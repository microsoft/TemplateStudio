using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.UI.V2Controls;
using Microsoft.Templates.UI.V2Resources;

namespace Microsoft.Templates.UI.V2Extensions
{
    public static class SyncStatusExtensions
    {
        public static Notification GetNotification(this SyncStatus status)
        {
            switch (status)
            {
                case SyncStatus.Updating:
                    return Notification.Information(StringRes.NotificationSyncStatus_Updating, Category.TemplatesSync, false);
                case SyncStatus.Updated:
                    return Notification.Information(StringRes.NotificationSyncStatus_Updated, Category.TemplatesSync, false);
                case SyncStatus.Acquiring:
                    return Notification.Information(StringRes.NotificationSyncStatus_Acquiring, Category.TemplatesSync, false);
                case SyncStatus.Acquired:
                    return Notification.Information(StringRes.NotificationSyncStatus_Acquired, Category.TemplatesSync, false);
                case SyncStatus.Preparing:
                    return Notification.Information(StringRes.NotificationSyncStatus_Preparing, Category.TemplatesSync, false);
                case SyncStatus.Prepared:
                    return Notification.Information(StringRes.NotificationSyncStatus_Prepared, Category.TemplatesSync, false);
                case SyncStatus.OverVersion:
                    return Notification.Information(StringRes.NotificationSyncStatus_OverVersion, Category.TemplatesSync, false);
                case SyncStatus.OverVersionNoContent:
                    return Notification.Information(StringRes.NotificationSyncStatus_OverVersionNoContent, Category.TemplatesSync, false);
                case SyncStatus.NewVersionAvailable:
                    return Notification.Information(StringRes.NotificationSyncStatus_NewVersionAvailable, Category.TemplatesSync, false);
                case SyncStatus.None:
                    return null;
                case SyncStatus.UnderVersion:
                    return null;
                default:
                    return null;
            }
        }
    }
}
