using System;
using Windows.UI.Xaml.Data;

namespace Param_RootNamespace.Converters
{
    public class DateTimeFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime dt && parameter != null)
            {
                return dt.ToString(parameter.ToString());
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                return DateTime.Parse(value.ToString());
            }

            return default(DateTime);
        }
    }
}
