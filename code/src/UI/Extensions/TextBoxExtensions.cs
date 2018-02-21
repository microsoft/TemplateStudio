// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Templates.UI.Services;

namespace Microsoft.Templates.UI.Extensions
{
    public class TextBoxExtensions
    {
        private static string _currentFocusTemplateName;

        public static readonly DependencyProperty IsTextSelectedProperty = DependencyProperty.RegisterAttached(
        "IsTextSelected",
        typeof(bool),
        typeof(TextBoxExtensions),
        new UIPropertyMetadata(false, OnIsTextSelectedPropertyChanged));

        public static readonly DependencyProperty LostKeyboardFocusCommandProperty = DependencyProperty.RegisterAttached(
          "LostKeyboardFocusCommand",
          typeof(ICommand),
          typeof(TextBoxExtensions),
          new PropertyMetadata(null, OnLostKeyboardFocusCommandPropertyChanged));

        public static bool GetIsTextSelected(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsTextSelectedProperty);
        }

        public static void SetIsTextSelected(DependencyObject obj, bool value)
        {
            obj.SetValue(IsTextSelectedProperty, value);
        }

        public static void SetLostKeyboardFocusCommand(UIElement element, ICommand value)
        {
            element.SetValue(LostKeyboardFocusCommandProperty, value);
        }

        public static ICommand GetLostKeyboardFocusCommand(UIElement element)
        {
            return (ICommand)element.GetValue(LostKeyboardFocusCommandProperty);
        }

        private static void OnIsTextSelectedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBox;

            if ((bool)e.NewValue)
            {
                textBox?.Focus();
                textBox.Select(0, textBox.Text.Length);
            }
        }

        private static void OnLostKeyboardFocusCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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
