// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Templates.Core.Locations
{
    public enum SyncStatus
    {
        None = 0,
        Updating = 1,
        Updated = 2,
        CheckingForUpdates = 3,
        NoUpdates = 4,
        Acquiring = 5,
        Acquired = 6,
        Preparing = 7,
        Prepared = 8,
        NewWizardVersionAvailable = 9,
        Ready = 10,
        ErrorAcquiring = 11,
        Copying = 12
    }
}
