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

using Microsoft.Templates.UI.Extensions;

namespace Microsoft.Templates.UI.Controls
{
    public sealed partial class TogglePane : Control
    {
        private Border _togglePaneShadowGrid;
        private Grid _menuGrid;
        private bool _isInitialized = false;

        public object TogglePaneContent
        {
            get => GetValue(TogglePaneContentProperty);
            set => SetValue(TogglePaneContentProperty, value);
        }
        public static readonly DependencyProperty TogglePaneContentProperty = DependencyProperty.Register("TogglePaneContent", typeof(object), typeof(TogglePane), new PropertyMetadata(null));

        public DataTemplate MainViewTemplate
        {
            get => (DataTemplate)GetValue(MainViewTemplateProperty);
            set => SetValue(MainViewTemplateProperty, value);
        }
        public static readonly DependencyProperty MainViewTemplateProperty = DependencyProperty.Register("MainViewTemplate", typeof(DataTemplate), typeof(TogglePane), new PropertyMetadata(null));

        public DataTemplateSelector MainViewTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(MainViewTemplateSelectorProperty);
            set => SetValue(MainViewTemplateSelectorProperty, value);
        }
        public static readonly DependencyProperty MainViewTemplateSelectorProperty = DependencyProperty.Register("MainViewTemplateSelector", typeof(DataTemplateSelector), typeof(TogglePane), new PropertyMetadata(null));

        public DataTemplate SecondaryViewTemplate
        {
            get => (DataTemplate)GetValue(SecondaryViewTemplateProperty);
            set => SetValue(SecondaryViewTemplateProperty, value);
        }
        public static readonly DependencyProperty SecondaryViewTemplateProperty = DependencyProperty.Register("SecondaryViewTemplate", typeof(DataTemplate), typeof(TogglePane), new PropertyMetadata(null));

        public DataTemplate OpenButtonTemplate
        {
            get => (DataTemplate)GetValue(OpenButtonTemplateProperty);
            set => SetValue(OpenButtonTemplateProperty, value);
        }
        public static readonly DependencyProperty OpenButtonTemplateProperty = DependencyProperty.Register("OpenButtonTemplate", typeof(DataTemplate), typeof(TogglePane), new PropertyMetadata(null));

        public DataTemplate CloseButtonTemplate
        {
            get => (DataTemplate)GetValue(CloseButtonTemplateProperty);
            set => SetValue(CloseButtonTemplateProperty, value);
        }
        public static readonly DependencyProperty CloseButtonTemplateProperty = DependencyProperty.Register("CloseButtonTemplate", typeof(DataTemplate), typeof(TogglePane), new PropertyMetadata(null));

        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(TogglePane), new PropertyMetadata(false, OnIsOpenPropertyChanged));

        public bool AllowDragAndDrop
        {
            get => (bool)GetValue(AllowDragAndDropProperty);
            set => SetValue(AllowDragAndDropProperty, value);
        }
        public static readonly DependencyProperty AllowDragAndDropProperty = DependencyProperty.Register("AllowDragAndDrop", typeof(bool), typeof(TogglePane), new PropertyMetadata(false));

        static TogglePane()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TogglePane), new FrameworkPropertyMetadata(typeof(TogglePane)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _togglePaneShadowGrid = GetTemplateChild("togglePaneShadowGrid") as Border;
            _menuGrid = GetTemplateChild("menuGrid") as Grid;

            _isInitialized = true;
            UpdateOpenStatus();
        }

        private void UpdateOpenStatus(bool newValue = false, bool oldValue = false)
        {
            if (_isInitialized)
            {
                if (IsOpen)
                {
                    _menuGrid.AnimateWidth(90);
                    _togglePaneShadowGrid.AnimateWidth(90);
                    _togglePaneShadowGrid.FadeIn();
                }
                else
                {
                    _menuGrid.AnimateWidth(30);
                    _togglePaneShadowGrid.AnimateWidth(30);
                    _togglePaneShadowGrid.FadeOut();
                }
            }
        }

        private static void OnIsOpenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as TogglePane;
            if (control != null)
            {
                control.UpdateOpenStatus((bool)e.NewValue, (bool)e.OldValue);
            }
        }
    }
}
