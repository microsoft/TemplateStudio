// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Microsoft.Templates.Core
{
    public class TemplateLicenseEqualityComparer : IEqualityComparer<TemplateLicense>
    {
        public bool Equals(TemplateLicense x, TemplateLicense y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            else if (x == null || y == null)
            {
                return false;
            }
            else if (x.Url == y.Url)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(TemplateLicense obj)
        {
            if (obj == null || string.IsNullOrEmpty(obj.Url))
            {
                return 0;
            }

            return obj.Url.GetHashCode();
        }
    }
}
