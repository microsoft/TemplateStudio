// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Templates.Core
{
    public static class VersionExtensions
    {
        public static bool IsZero(this Version v)
        {
            return !v.IsNull() && v.Major <= 0 && v.Minor <= 0 && v.Build <= 0 && v.Revision <= 0;
        }

        public static bool IsNull(this Version v)
        {
            return v is null;
        }
    }
}
