// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Microsoft.Templates.UI.Extensions;
using Microsoft.Templates.UI.ViewModels.NewProject;
using Microsoft.Templates.UI.Views.NewProject;

namespace Microsoft.Templates.UI.Controls
{
    public partial class TogglePanel : UserControl
    {
        public object TogglePanelContent
        {
            get => GetValue(TogglePanelContentProperty);
            set => SetValue(TogglePanelContentProperty, value);
        }
        public static readonly DependencyProperty TogglePanelContentProperty = DependencyProperty.Register(nameof(TogglePanelContent), typeof(object), typeof(TogglePanel), new PropertyMetadata(null));

        public DataTemplate MainViewTemplate
        {
            get => (DataTemplate)GetValue(MainViewTemplateProperty);
            set => SetValue(MainViewTemplateProperty, value);
        }
        public static readonly DependencyProperty MainViewTemplateProperty = DependencyProperty.Register(nameof(MainViewTemplate), typeof(DataTemplate), typeof(TogglePanel), new PropertyMetadata(null));

        public DataTemplate SecondaryViewTemplate
        {
            get => (DataTemplate)GetValue(SecondaryViewTemplateProperty);
            set => SetValue(SecondaryViewTemplateProperty, value);
        }
        public static readonly DependencyProperty SecondaryViewTemplateProperty = DependencyProperty.Register(nameof(SecondaryViewTemplate), typeof(DataTemplate), typeof(TogglePanel), new PropertyMetadata(null));

        public DataTemplate OpenButtonTemplate
        {
            get => (DataTemplate)GetValue(OpenButtonTemplateProperty);
            set => SetValue(OpenButtonTemplateProperty, value);
        }
        public static readonly DependencyProperty OpenButtonTemplateProperty = DependencyProperty.Register(nameof(OpenButtonTemplate), typeof(DataTemplate), typeof(TogglePanel), new PropertyMetadata(null));

        public DataTemplate CloseButtonTemplate
        {
            get => (DataTemplate)GetValue(CloseButtonTemplateProperty);
            set => SetValue(CloseButtonTemplateProperty, value);
        }
        public static readonly DependencyProperty CloseButtonTemplateProperty = DependencyProperty.Register(nameof(CloseButtonTemplate), typeof(DataTemplate), typeof(TogglePanel), new PropertyMetadata(null));

        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(TogglePanel), new PropertyMetadata(false, OnIsOpenPropertyChanged));

        public bool AllowDragAndDrop
        {
            get => (bool)GetValue(AllowDragAndDropProperty);
            set => SetValue(AllowDragAndDropProperty, value);
        }
        public static readonly DependencyProperty AllowDragAndDropProperty = DependencyProperty.Register(nameof(AllowDragAndDrop), typeof(bool), typeof(TogglePanel), new PropertyMetadata(false));

        public TogglePanel()
        {
            InitializeComponent();
            MainView.Current.KeyDown += OnMainViewKeyDown;
            UpdateOpenStatus();
        }

        private void OnMainGridGotFocus(object sender, RoutedEventArgs e) => MainViewModel.Current.Ordering.SavedTemplateGotFocus(TogglePanelContent as SavedTemplateViewModel);

        private static void OnIsOpenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as TogglePanel;
            if (control != null)
            {
                control.UpdateOpenStatus((bool)e.NewValue, (bool)e.OldValue);
            }
        }

        private void OnMainViewKeyDown(object sender, KeyEventArgs e)
        {
            if (!AllowDragAndDrop)
            {
                return;
            }
            if (e.Key == Key.Space && mainGrid.IsFocused)
            {
                if (MainViewModel.Current.Ordering.SavedTemplateSetDrag(TogglePanelContent as SavedTemplateViewModel))
                {
                    dragAndDropShadowBorder.Opacity = 1;
                }
                else
                {
                    MainViewModel.Current.Ordering.SavedTemplateSetDrop(TogglePanelContent as SavedTemplateViewModel);
                }
            }
            if (e.Key == Key.Escape)
            {
                dragAndDropShadowBorder.Opacity = 0;
            }
        }

        private void UpdateOpenStatus(bool newValue = false, bool oldValue = false)
        {
            if (IsOpen)
            {
                menuGrid.AnimateWidth(90);
                togglePaneShadowGrid.AnimateWidth(90);
                togglePaneShadowGrid.FadeIn();
            }
            else
            {
                menuGrid.AnimateWidth(30);
                togglePaneShadowGrid.AnimateWidth(30);
                togglePaneShadowGrid.FadeOut();
            }
        }
    }
}
