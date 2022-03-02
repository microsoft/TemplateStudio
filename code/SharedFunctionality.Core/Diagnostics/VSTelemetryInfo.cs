// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Templates.Core.Diagnostics
{
    public class VSTelemetryInfo
    {
        public bool OptedIn { get; set; }

        public string VisualStudioEdition { get; set; }

        public string VisualStudioExeVersion { get; set; }

        public string VisualStudioCulture { get; set; }

        public string VisualStudioManifestId { get; set; }
    }
}
