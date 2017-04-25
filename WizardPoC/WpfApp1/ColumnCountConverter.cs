using System;
using System.Windows.Data;

namespace WpfApp1
{
    public class ColumnCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var width = System.Convert.ToDouble(value);
            var targetWidth = System.Convert.ToDouble(parameter);

            return width > targetWidth ? 2 : 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
