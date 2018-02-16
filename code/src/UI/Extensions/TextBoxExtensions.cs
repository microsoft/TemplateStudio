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

        public static readonly DependencyProperty ListenIsFocusedProperty = DependencyProperty.RegisterAttached(
          "ListenIsFocused",
          typeof(bool),
          typeof(TextBoxExtensions),
          new PropertyMetadata(false, OnListenIsFocusedPropertyChanged));

        public static readonly DependencyProperty LostKeyboardFocusCommandProperty = DependencyProperty.RegisterAttached(
          "LostKeyboardFocusCommand",
          typeof(ICommand),
          typeof(TextBoxExtensions),
          new PropertyMetadata(null, OnLostKeyboardFocusCommandPropertyChanged));

        public static void SetListenIsFocused(UIElement element, bool value)
        {
            element.SetValue(ListenIsFocusedProperty, value);
        }

        public static bool GetListenIsFocused(UIElement element)
        {
            return (bool)element.GetValue(ListenIsFocusedProperty);
        }

        public static void SetLostKeyboardFocusCommand(UIElement element, ICommand value)
        {
            element.SetValue(LostKeyboardFocusCommandProperty, value);
        }

        public static ICommand GetLostKeyboardFocusCommand(UIElement element)
        {
            return (ICommand)element.GetValue(LostKeyboardFocusCommandProperty);
        }

        private static void OnListenIsFocusedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBox;
            if (textBox != null)
            {
                EventService.Instance.OnSavedTemplateFocused += (sender, templateName) =>
                {
                    _currentFocusTemplateName = templateName;
                    ProcessFocus(textBox);
                };
                ProcessFocus(textBox);
            }
        }

        private static void ProcessFocus(TextBox textBox)
        {
            if (textBox.Text == _currentFocusTemplateName)
            {
                textBox.Focus();
                textBox.Select(0, textBox.Text.Length);
                _currentFocusTemplateName = string.Empty;
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
