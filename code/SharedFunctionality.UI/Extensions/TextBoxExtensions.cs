// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Templates.UI.Extensions
{
    public class TextBoxExtensions
    {
        public static readonly DependencyProperty IsTextSelectedProperty = DependencyProperty.RegisterAttached(
        "IsTextSelected",
        typeof(bool),
        typeof(TextBoxExtensions),
        new UIPropertyMetadata(false, OnIsTextSelectedPropertyChanged));

        public static bool GetIsTextSelected(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsTextSelectedProperty);
        }

        public static void SetIsTextSelected(DependencyObject obj, bool value)
        {
            obj.SetValue(IsTextSelectedProperty, value);
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
    }
}
