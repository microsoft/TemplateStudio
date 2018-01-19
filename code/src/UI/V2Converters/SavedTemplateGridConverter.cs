// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Windows.Data;
using Microsoft.Templates.UI.V2Services;

namespace Microsoft.Templates.UI.V2Converters
{
    public class SavedTemplateGridConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool hasErrors = false;
            if (value != null)
            {
                bool.TryParse(value.ToString(), out hasErrors);
            }

            var styleName = hasErrors ? "WtsGridSavedTemplateError" : "WtsGridSavedTemplate";
            return ResourcesService.Instance.FindResource(styleName);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
