// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.Extensions
{
    public static class SyncStatusExtensions
    {
        public static StatusViewModel GetStatusViewModel(this SyncStatus status, Version version)
        {
            if (status == SyncStatus.None || status == SyncStatus.Ready)
            {
                return StatusViewModel.EmptyStatus;
            }

            return new StatusViewModel(status.GetStatusType(), status.GetResourceString(version), true, status.GetStatusHideSeconds());
        }

        private static StatusType GetStatusType(this SyncStatus status)
        {
            switch (status)
            {
                case SyncStatus.NewWizardVersionAvailable:
                    return StatusType.Warning;
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

        private static string GetResourceString(this SyncStatus status, Version version)
        {
            switch (status)
            {
                case SyncStatus.Updating:
                    return StringRes.StatusUpdating;
                case SyncStatus.Updated:
                    return StringRes.StatusUpdated;
                case SyncStatus.Acquiring:
                    return string.Format(StringRes.StatusAcquiring, version);
                case SyncStatus.Acquired:
                    return StringRes.StatusAcquired;
                case SyncStatus.Preparing:
                    return string.Format(StringRes.StatusPreparing, version);
                case SyncStatus.Prepared:
                    return StringRes.StatusPrepared;
                case SyncStatus.NewWizardVersionAvailable:
                    return string.Format(StringRes.StatusNewWizardVersionAvailable, version);
                case SyncStatus.CheckingForUpdates:
                    return StringRes.StatusCheckingForUpdates;
                case SyncStatus.CheckedForUpdates:
                    return StringRes.StatusCheckedForUpdates;
                default:
                    return string.Empty;
            }
        }
    }
}
