using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Microsoft.Templates.Wizard.Converters
{
    public class ListToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var collection = value as IEnumerable<string>;
            if (collection == null)
            {
                return value;
            }
            return string.Join(GetParam(parameter), collection.ToArray());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private string GetParam(object parameter)
        {
            var separator = parameter as string;
            if (string.IsNullOrEmpty(separator))
            {
                separator = ", ";
            }
            return separator;
        }
    }
}
