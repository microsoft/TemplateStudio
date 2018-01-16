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
        Acquiring = 3,
        Acquired = 4,
        Preparing = 5,
        Prepared = 6,
        NewWizardVersionAvailable = 7,
        CheckingForUpdates = 8,
        CheckedForUpdates = 9,
        Ready = 10
    }
}
