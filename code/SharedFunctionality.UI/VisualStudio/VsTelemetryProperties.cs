// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Templates.UI.VisualStudio
{
    public class VsTelemetryProperties
    {
        public const string Prefix = "Wts.";

        public static string Pages { get; private set; } = Prefix + "Pages";

        public static string Features { get; private set; } = Prefix + "Features";
    }
}
