// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Microsoft.Templates.Core
{
    public static class Platforms
    {
        public const string Uwp = "Uwp";

        public static IEnumerable<string> GetAllPlatforms()
        {
            yield return Platforms.Uwp;
        }
    }
}
