using System;
using System.Windows.Media;

using WUXD = Windows.UI.Xaml.Data;

using WUXM = Windows.UI.Xaml.Media;

namespace DotNetCoreWpfApp.Converters
{
    public class ColorConverter : WUXD.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is SolidColorBrush brush)
            {
                return FromSystemColor(brush);
            }

            return default;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is WUXM.SolidColorBrush brush)
            {
                return FromWindowsColor(brush);
            }

            return default;
        }

        public static WUXM.SolidColorBrush FromSystemColor(SolidColorBrush brush)
            => new WUXM.SolidColorBrush(new Windows.UI.Color()
            {
                A = brush.Color.A,
                R = brush.Color.R,
                G = brush.Color.G,
                B = brush.Color.B
            });

        public static SolidColorBrush FromWindowsColor(WUXM.SolidColorBrush brush)
            => new SolidColorBrush(new Color()
            {
                A = brush.Color.A,
                R = brush.Color.R,
                G = brush.Color.G,
                B = brush.Color.B
            });
    }
}
