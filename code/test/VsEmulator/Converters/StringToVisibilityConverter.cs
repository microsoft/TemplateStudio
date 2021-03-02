using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Microsoft.Templates.VsEmulator.Converters
{
    public class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isNullString = value == null || string.IsNullOrEmpty(value.ToString());
            if (parameter == null)
            {
                return isNullString ? Visibility.Collapsed : Visibility.Visible;
            }
            else
            {
                return isNullString ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
