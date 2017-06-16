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

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Microsoft.Templates.UI.Controls
{
    public class TextBoxEx : TextBox
    {
        ////public bool IsEditionEnabled
        ////{
        ////    get { return (bool)GetValue(IsEditionEnabledProperty); }
        ////    set { SetValue(IsEditionEnabledProperty, value); }
        ////}
        ////public static readonly DependencyProperty IsEditionEnabledProperty = DependencyProperty.Register("IsEditionEnabled", typeof(bool), typeof(TextBoxEx), new PropertyMetadata(false));

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

        ////private void LostFocusHandler(object sender, RoutedEventArgs e)
        ////{
        ////    //MainViewModel.Current.ProjectTemplates.CloseTemplatesEdition();
        ////    //MainViewModel.Current.ProjectTemplates.CloseSummaryItemsEdition();
        ////}

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

        ////protected override void OnLostFocus(RoutedEventArgs e)
        ////{
        ////    base.OnLostFocus(e);
        ////    //MainViewModel.Current.MainView.Dispatcher.Invoke(() =>
        ////    //{
        ////    //    MainViewModel.Current.ProjectTemplates.CloseTemplatesEdition();
        ////    //    MainViewModel.Current.ProjectTemplates.CloseSummaryItemsEdition();
        ////    //});
        ////}

        ////protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        ////{
        ////    base.OnLostKeyboardFocus(e);
        ////    //MainViewModel.Current.MainView.Dispatcher.Invoke(() =>
        ////    //{
        ////    //    MainViewModel.Current.ProjectTemplates.CloseTemplatesEdition();
        ////    //    MainViewModel.Current.ProjectTemplates.CloseSummaryItemsEdition();
        ////    //});
        ////}

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
    }
}
