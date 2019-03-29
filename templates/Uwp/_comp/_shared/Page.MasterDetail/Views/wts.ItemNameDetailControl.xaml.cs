using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Param_RootNamespace.Core.Models;

namespace Param_RootNamespace.Views
{
    public sealed partial class wts.ItemNameDetailControl : UserControl
    {
        public SampleOrder MasterMenuItem
        {
            get { return GetValue(MasterMenuItemProperty) as SampleOrder; }
            set { SetValue(MasterMenuItemProperty, value); }
        }

        public static readonly DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem", typeof(SampleOrder), typeof(wts.ItemNameDetailControl), new PropertyMetadata(null, OnMasterMenuItemPropertyChanged));

        public wts.ItemNameDetailControl()
        {
            InitializeComponent();
        }

        private static void OnMasterMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as wts.ItemNameDetailControl;
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}
