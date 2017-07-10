using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Microsoft.Templates.UI.Controls
{
    public class LogoControl : Control
    {
        private static Brush defaultFill = new SolidColorBrush(Colors.White);
        public Brush Fill
        {
            get => (Brush)GetValue(FillProperty);
            set => SetValue(FillProperty, value);
        }
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register("Fill", typeof(Brush), typeof(LogoControl), new PropertyMetadata(defaultFill));

        public LogoControl()
        {
            DefaultStyleKey = typeof(LogoControl);
        }
    }
}
