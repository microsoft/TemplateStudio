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
        Preparing = 6,
        NewWizardVersionAvailable = 7,
        Ready = 8,
        ErrorAcquiring = 9,
    }
}
