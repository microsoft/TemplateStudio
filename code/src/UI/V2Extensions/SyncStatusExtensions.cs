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
                    return Notification.Information(StringRes.NotificationSyncStatus_Updating, Category.TemplatesSync);
                case SyncStatus.Updated:
                    return Notification.Information(StringRes.NotificationSyncStatus_Updated, Category.TemplatesSync);
                case SyncStatus.Acquiring:
                    return Notification.Information(StringRes.NotificationSyncStatus_Acquiring, Category.TemplatesSync);
                case SyncStatus.Acquired:
                    return Notification.Information(StringRes.NotificationSyncStatus_Acquired, Category.TemplatesSync);
                case SyncStatus.Preparing:
                    return Notification.Information(StringRes.NotificationSyncStatus_Preparing, Category.TemplatesSync);
                case SyncStatus.Prepared:
                    return Notification.Information(StringRes.NotificationSyncStatus_Prepared, Category.TemplatesSync);
                case SyncStatus.OverVersion:
                    return Notification.Information(StringRes.NotificationSyncStatus_OverVersion, Category.TemplatesSync);
                case SyncStatus.OverVersionNoContent:
                    return Notification.Information(StringRes.NotificationSyncStatus_OverVersionNoContent, Category.TemplatesSync);
                case SyncStatus.NewVersionAvailable:
                    return Notification.Information(StringRes.NotificationSyncStatus_NewVersionAvailable, Category.TemplatesSync);
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
