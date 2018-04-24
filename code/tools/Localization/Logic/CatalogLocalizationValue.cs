// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Localization
{
    public class CatalogLocalizationValue
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Summary { get; set; }

        public override bool Equals(object obj)
        {
            var item = obj as CatalogLocalizationValue;

            if (item == null)
            {
                return false;
            }

            return item.Name == Name &&
                item.DisplayName == DisplayName &&
                item.Summary == Summary;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
