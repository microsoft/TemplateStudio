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
                    return Notification.Information(StringRes.NotificationSyncStatus_Updating, ReplacementCategory.TemplatesSync);
                case SyncStatus.Updated:
                    return Notification.Information(StringRes.NotificationSyncStatus_Updated, ReplacementCategory.TemplatesSync);
                case SyncStatus.Acquiring:
                    return Notification.Information(StringRes.NotificationSyncStatus_Acquiring, ReplacementCategory.TemplatesSync);
                case SyncStatus.Acquired:
                    return Notification.Information(StringRes.NotificationSyncStatus_Acquired, ReplacementCategory.TemplatesSync);
                case SyncStatus.Preparing:
                    return Notification.Information(StringRes.NotificationSyncStatus_Preparing, ReplacementCategory.TemplatesSync);
                case SyncStatus.Prepared:
                    return Notification.Information(StringRes.NotificationSyncStatus_Prepared, ReplacementCategory.TemplatesSync);
                case SyncStatus.OverVersion:
                    return Notification.Information(StringRes.NotificationSyncStatus_OverVersion, ReplacementCategory.TemplatesSync);
                case SyncStatus.OverVersionNoContent:
                    return Notification.Information(StringRes.NotificationSyncStatus_OverVersionNoContent, ReplacementCategory.TemplatesSync);
                case SyncStatus.NewVersionAvailable:
                    return Notification.Information(StringRes.NotificationSyncStatus_NewVersionAvailable, ReplacementCategory.TemplatesSync);
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
