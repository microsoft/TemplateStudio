// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Microsoft.Templates.UI.V2ViewModels.NewProject
{
    internal class ColorComparer : IEqualityComparer<ColorItem>
    {
        public bool Equals(ColorItem x, ColorItem y)
        {
            return x.Code == y.Code && x.Name == y.Name;
        }

        public int GetHashCode(ColorItem obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
