// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Templates.Extension.Commands
{
    internal class PackageIds
    {
        public const int AddPageCommand = 0x0400;
        public const int AddFeatureCommand = 0x500;
        public const int AddServiceCommand = 0x0600;
        public const int AddTestingCommand = 0x0700;
        public const int OpenTempFolder = 0x800;
    }

    [SuppressMessage("StyleCop", "SA1402", Justification = "This class does not have implementation. Used for constants.")]
    internal class PackageGuids
    {
        /// <summary>
        /// RelayCommandPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "ae1b4c32-9c93-45b8-a36b-8734f4b120dd";

        public static Guid GuidRelayCommandPackageCmdSet { get; } = Guid.Parse("caa4fb82-0dca-40fe-bae0-081e0f96226f");
    }
}
