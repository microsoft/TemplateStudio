// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Microsoft.Templates.UI.Controls
{
    public class TextBoxEx : TextBox
    {
        public bool ForceSetFocus
        {
            get { return (bool)GetValue(IsFocusedProperty); }
            set { SetValue(IsFocusedProperty, value); }
        }

        public static readonly DependencyProperty ForceSetFocusProperty = DependencyProperty.Register("ForceSetFocus", typeof(bool), typeof(TextBoxEx), new PropertyMetadata(false, OnForceSetFocusPropertyChanged));

        private static void OnForceSetFocusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as TextBoxEx;
            control.UpdateFocus((bool)e.NewValue);
        }

        public TextBoxEx()
        {
            FocusManager.AddGotFocusHandler(this, GotFocusHandler);

            // FocusManager.AddLostFocusHandler(this, LostFocusHandler);
        }

        private void UpdateFocus(bool force)
        {
            if (force)
            {
                var focusScope = FocusManager.GetFocusScope(this);
                FocusManager.SetFocusedElement(focusScope, this);
            }
        }

        private void GotFocusHandler(object sender, RoutedEventArgs e)
        {
            Select(0, Text.Length);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
    }
}
