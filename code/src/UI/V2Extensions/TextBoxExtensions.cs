using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        public static readonly DependencyProperty TextChangedCommandProperty = DependencyProperty.RegisterAttached(
          "TextChangedCommand",
          typeof(ICommand),
          typeof(TextBoxExtensions),
          new PropertyMetadata(null, OnTextChangedCommandPropertyChanged)
        );

        public static readonly DependencyProperty LostKeyboardFocusCommandProperty = DependencyProperty.RegisterAttached(
          "LostKeyboardFocusCommand",
          typeof(ICommand),
          typeof(TextBoxExtensions),
          new PropertyMetadata(null, LostKeyboardFocusCommandPropertyChanged)
        );

        public static void SetIsFocused(UIElement element, bool value)
        {
            element.SetValue(IsFocusedProperty, value);
        }

        public static bool GetIsFocused(UIElement element)
        {
            return (bool)element.GetValue(IsFocusedProperty);
        }

        public static void SetTextChangedCommand(UIElement element, ICommand value)
        {
            element.SetValue(TextChangedCommandProperty, value);
        }

        public static ICommand GetTextChangedCommand(UIElement element)
        {
            return (ICommand)element.GetValue(TextChangedCommandProperty);
        }

        public static void SetLostKeyboardFocusCommand(UIElement element, ICommand value)
        {
            element.SetValue(LostKeyboardFocusCommandProperty, value);
        }

        public static ICommand GetLostKeyboardFocusCommand(UIElement element)
        {
            return (ICommand)element.GetValue(LostKeyboardFocusCommandProperty);
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

        private static void OnTextChangedCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBox;
            if (textBox != null)
            {
                textBox.TextChanged += (sender, args) =>
                {
                    GetTextChangedCommand(textBox).Execute(args);
                };
            }
        }

        private static void LostKeyboardFocusCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBox;
            if (textBox != null)
            {
                textBox.LostKeyboardFocus += (sender, args) =>
                {
                    GetLostKeyboardFocusCommand(textBox).Execute(args);
                };
            }
        }
    }
}
