using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Templates.UI.V2Extensions
{
    public class TextBoxExtensions
    {
        public static readonly DependencyProperty IsFocusedProperty = DependencyProperty.RegisterAttached(
          "IsFocused",
          typeof(bool),
          typeof(TextBoxExtensions),
          new PropertyMetadata(false, OnIsFocusedPropertyChanged)
        );

        // new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender)

        public static void SetIsFocused(UIElement element, bool value)
        {
            element.SetValue(IsFocusedProperty, value);
        }

        public static bool GetIsFocused(UIElement element)
        {
            return (bool)element.GetValue(IsFocusedProperty);
        }

        private static void OnIsFocusedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBox;
            if (textBox != null)
            {
                if ((bool)e.NewValue)
                {
                    textBox.Focus();
                    textBox.Select(0, textBox.Text.Length);
                }
            }
        }
    }
}
