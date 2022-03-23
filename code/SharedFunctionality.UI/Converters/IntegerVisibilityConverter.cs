// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Microsoft.Templates.UI.Converters
{
    public class IntegerVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var intValue = GetToIntValue(value);
            if (parameter == null)
            {
                return intValue > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                return intValue > 0 ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private int GetToIntValue(object value)
        {
            int intValue = 0;
            if (value is int i)
            {
                intValue = i;
            }

            return intValue;
        }
    }
}
