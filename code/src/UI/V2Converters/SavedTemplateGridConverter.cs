using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Microsoft.Templates.UI.V2Views.NewProject;

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
            return WizardShell.Current.FindResource(styleName);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
