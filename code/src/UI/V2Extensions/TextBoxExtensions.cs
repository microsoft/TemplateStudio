// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Templates.UI.V2Services;

namespace Microsoft.Templates.UI.V2Extensions
{
    public class TextBoxExtensions
    {
        private static string _currentFocusTemplateName;

        public static readonly DependencyProperty ListenIsFocusedProperty = DependencyProperty.RegisterAttached(
          "ListenIsFocused",
          typeof(bool),
          typeof(TextBoxExtensions),
          new PropertyMetadata(false, OnListenIsFocusedPropertyChanged));

        public static readonly DependencyProperty TextChangedCommandProperty = DependencyProperty.RegisterAttached(
          "TextChangedCommand",
          typeof(ICommand),
          typeof(TextBoxExtensions),
          new PropertyMetadata(null, OnTextChangedCommandPropertyChanged));

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

        private static void OnListenIsFocusedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBox;
            if (textBox != null)
            {
                EventService.Instance.OnSavedTemplateFocused += (sender, template) =>
                {
                    _currentFocusTemplateName = template.Name;
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


        public static readonly DependencyProperty ReturnKeySetItemFocusProperty = DependencyProperty.RegisterAttached(
          "ReturnKeySetItemFocus",
          typeof(bool),
          typeof(TextBoxExtensions),
          new PropertyMetadata(false, OnReturnKeySetItemFocusPropertyChanged));

        public static void SetReturnKeySetItemFocus(UIElement element, bool value)
        {
            element.SetValue(ReturnKeySetItemFocusProperty, value);
        }

        public static bool GetReturnKeySetItemFocus(UIElement element)
        {
            return (bool)element.GetValue(ReturnKeySetItemFocusProperty);
        }

        private static void OnReturnKeySetItemFocusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBox;
            if (textBox != null)
            {
                textBox.PreviewKeyDown += TextBox_PreviewKeyDown;
            }
        }

        private static void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var textbox = e.OriginalSource as TextBox;
            if (e.Key == Key.Return)
            {
                e.Handled = true;
                textbox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
            }
        }
    }
}
