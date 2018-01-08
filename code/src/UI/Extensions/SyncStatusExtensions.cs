// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.Extensions
{
    public static class SyncStatusExtensions
    {
        public static StatusViewModel GetStatusViewModel(this SyncStatus status)
        {
            if (status == SyncStatus.None)
            {
                return StatusViewModel.EmptyStatus;
            }

            return new StatusViewModel(status.GetStatusType(), status.GetResourceString(), true, status.GetStatusHideSeconds());
        }

        private static StatusType GetStatusType(this SyncStatus status)
        {
            switch (status)
            {
                case SyncStatus.NewWizardVersionAvailable:
                    return StatusType.Warning;

                case SyncStatus.OverVersionNoContent:
                case SyncStatus.UnderVersion:
                    return StatusType.Error;

                default:
                    return StatusType.Information;
            }
        }

        private static int GetStatusHideSeconds(this SyncStatus status)
        {
            switch (status)
            {
                case SyncStatus.Acquiring:
                    return 0;
                default:
                    return 5;
            }
        }

        private static string GetResourceString(this SyncStatus status)
        {
            switch (status)
            {
                case SyncStatus.Updating:
                    return StringRes.StatusUpdating;
                case SyncStatus.Updated:
                    return StringRes.StatusUpdated;
                case SyncStatus.Acquiring:
                    return StringRes.StatusAcquiring;
                case SyncStatus.Acquired:
                    return StringRes.StatusAcquired;
                case SyncStatus.Preparing:
                    return StringRes.StatusPreparing;
                case SyncStatus.Prepared:
                    return StringRes.StatusPrepared;
                case SyncStatus.NewVersionAvailable:
                    return StringRes.StatusNewVersionAvailable;
                case SyncStatus.NewWizardVersionAvailable:
                    return StringRes.StatusOverVersionContent;
                case SyncStatus.OverVersionNoContent:
                    return StringRes.StatusOverVersionNoContent;
                default:
                    return string.Empty;
            }
        }
    }
}
