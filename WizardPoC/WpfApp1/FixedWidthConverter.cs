using System;
using System.Windows;
using System.Windows.Data;

namespace WpfApp1
{
    public class FixedWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (HorizontalAlignment)value == HorizontalAlignment.Stretch ? double.NaN : parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
