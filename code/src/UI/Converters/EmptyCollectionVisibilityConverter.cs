using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Microsoft.Templates.UI.Converters
{
    public class EmptyCollectionVisibilityConverter : IValueConverter
    {
        private static Dictionary<object, ObservableCollection<object>> _instances = new Dictionary<object, ObservableCollection<object>>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var list = value as IList;

            return list?.Count == 0 == false ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
