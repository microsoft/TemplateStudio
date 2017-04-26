using System;
using System.Windows.Data;

namespace WpfApp1
{
    public class ItemAlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (int)value > 1 ? System.Windows.HorizontalAlignment.Stretch : System.Windows.HorizontalAlignment.Left;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
