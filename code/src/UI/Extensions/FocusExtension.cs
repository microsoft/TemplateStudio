// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Templates.UI
{
    public static class FocusExtension
    {
        public static bool? GetIsFocused(DependencyObject obj) => (bool)obj.GetValue(IsFocusedProperty);
        public static void SetIsFocused(DependencyObject obj, bool value) => obj.SetValue(IsFocusedProperty, value);

        public static readonly DependencyProperty IsFocusedProperty = DependencyProperty.RegisterAttached("IsFocused", typeof(bool), typeof(FocusExtension), new PropertyMetadata(OnIsFocusedPropertyChanged));



        public static Action GetLostFocusAction(DependencyObject obj) => (Action)obj.GetValue(LostFocusActionProperty);
        public static void SetLostFocusAction(DependencyObject obj, Action value) => obj.SetValue(LostFocusActionProperty, value);

        public static readonly DependencyProperty LostFocusActionProperty = DependencyProperty.RegisterAttached("LostFocusAction", typeof(Action), typeof(FocusExtension), new PropertyMetadata(OnLostFocusActionPropertyChanged));

        private static void OnLostFocusActionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBox;
            Action action = e.NewValue as Action;

            if (textBox != null && action != null)
            {
                textBox.LostFocus += (sender, args) => action?.Invoke();
            }
        }

        private static void OnIsFocusedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBox;
            if (textBox != null && (bool)e.NewValue)
            {
                textBox.Focus();
                textBox.SelectAll();
            }
        }
    }
}
