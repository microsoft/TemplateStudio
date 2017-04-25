using System;
using System.Globalization;
using System.Windows.Data;

namespace Microsoft.Templates.UI.Converters
{
    public class ColumnCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var width = System.Convert.ToDouble(value);
            var targetWidth = System.Convert.ToDouble(parameter);

            return (width > targetWidth) ? 2 : 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}