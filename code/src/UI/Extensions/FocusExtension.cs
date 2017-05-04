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
