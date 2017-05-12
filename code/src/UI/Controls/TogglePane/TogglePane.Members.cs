using System;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Templates.UI.Controls
{
    public sealed partial class TogglePane
    {
        #region TogglePaneContent
        public object TogglePaneContent
        {
            get { return GetValue(TogglePaneContentProperty); }
            set { SetValue(TogglePaneContentProperty, value); }
        }
        public static readonly DependencyProperty TogglePaneContentProperty = DependencyProperty.Register("TogglePaneContent", typeof(object), typeof(TogglePane), new PropertyMetadata(null));
        #endregion

        #region MainViewTemplate
        public DataTemplate MainViewTemplate
        {
            get { return (DataTemplate)GetValue(MainViewTemplateProperty); }
            set { SetValue(MainViewTemplateProperty, value); }
        }
        public static readonly DependencyProperty MainViewTemplateProperty = DependencyProperty.Register("MainViewTemplate", typeof(DataTemplate), typeof(TogglePane), new PropertyMetadata(null));
        #endregion

        #region MainViewTemplateSelector
        public DataTemplateSelector MainViewTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(MainViewTemplateSelectorProperty); }
            set { SetValue(MainViewTemplateSelectorProperty, value); }
        }
        public static readonly DependencyProperty MainViewTemplateSelectorProperty = DependencyProperty.Register("MainViewTemplateSelector", typeof(DataTemplateSelector), typeof(TogglePane), new PropertyMetadata(null));
        #endregion

        #region SecondaryViewTemplate
        public DataTemplate SecondaryViewTemplate
        {
            get { return (DataTemplate)GetValue(SecondaryViewTemplateProperty); }
            set { SetValue(SecondaryViewTemplateProperty, value); }
        }
        public static readonly DependencyProperty SecondaryViewTemplateProperty = DependencyProperty.Register("SecondaryViewTemplate", typeof(DataTemplate), typeof(TogglePane), new PropertyMetadata(null));
        #endregion

        #region SecondaryViewTemplateSelector
        public DataTemplateSelector SecondaryViewTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(SecondaryViewTemplateSelectorProperty); }
            set { SetValue(SecondaryViewTemplateSelectorProperty, value); }
        }
        public static readonly DependencyProperty SecondaryViewTemplateSelectorProperty = DependencyProperty.Register("SecondaryViewTemplateSelector", typeof(DataTemplateSelector), typeof(TogglePane), new PropertyMetadata(null));
        #endregion

        #region ButtonTemplate
        public DataTemplate ButtonTemplate
        {
            get { return (DataTemplate)GetValue(ButtonTemplateProperty); }
            set { SetValue(ButtonTemplateProperty, value); }
        }
        public static readonly DependencyProperty ButtonTemplateProperty = DependencyProperty.Register("ButtonTemplate", typeof(DataTemplate), typeof(TogglePane), new PropertyMetadata(null));
        #endregion        

        #region CloseButtonTemplate
        public DataTemplate CloseButtonTemplate
        {
            get { return (DataTemplate)GetValue(CloseButtonTemplateProperty); }
            set { SetValue(CloseButtonTemplateProperty, value); }
        }
        public static readonly DependencyProperty CloseButtonTemplateProperty = DependencyProperty.Register("CloseButtonTemplate", typeof(DataTemplate), typeof(TogglePane), new PropertyMetadata(null));
        #endregion        

        #region IsOpen
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(TogglePane), new PropertyMetadata(false, OnIsOpenPropertyChanged));

        private static void OnIsOpenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as TogglePane;
            if (control != null)
            {
                control.UpdateOpenStatus();
            }
        }
        #endregion

        #region MouseEnterAction
        public Action MouseEnterAction
        {
            get { return (Action)GetValue(MouseEnterActionProperty); }
            set { SetValue(MouseEnterActionProperty, value); }
        }

        public static readonly DependencyProperty MouseEnterActionProperty = DependencyProperty.Register("MouseEnterAction", typeof(Action), typeof(TogglePane), new PropertyMetadata(null));
        #endregion

        #region MouseLeaveAction
        public Action MouseLeaveAction
        {
            get { return (Action)GetValue(MouseLeaveActionProperty); }
            set { SetValue(MouseLeaveActionProperty, value); }
        }

        public static readonly DependencyProperty MouseLeaveActionProperty = DependencyProperty.Register("MouseLeaveAction", typeof(Action), typeof(TogglePane), new PropertyMetadata(null));
        #endregion
    }
}
