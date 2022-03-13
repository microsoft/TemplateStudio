using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Param_RootNamespace.Core.Models;

namespace Param_RootNamespace.Views
{
    public sealed partial class wts.ItemNameDetailControl : UserControl
    {
        public SampleOrder ListMenuItem
        {
            get { return GetValue(ListMenuItemProperty) as SampleOrder; }
            set { SetValue(ListMenuItemProperty, value); }
        }

        public static readonly DependencyProperty ListMenuItemProperty = DependencyProperty.Register("ListMenuItem", typeof(SampleOrder), typeof(wts.ItemNameDetailControl), new PropertyMetadata(null, OnListMenuItemPropertyChanged));

        public wts.ItemNameDetailControl()
        {
            InitializeComponent();
        }

        private static void OnListMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as wts.ItemNameDetailControl;
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}
