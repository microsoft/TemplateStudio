using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using XamarinUwpNative.UWP.Models;

namespace XamarinUwpNative.UWP.Views
{
    public sealed partial class ListViewDetailControl : UserControl
    {
        public SampleOrder MasterMenuItem
        {
            get { return GetValue(MasterMenuItemProperty) as SampleOrder; }
            set { SetValue(MasterMenuItemProperty, value); }
        }

        public static readonly DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem", typeof(SampleOrder), typeof(ListViewDetailControl), new PropertyMetadata(null, OnMasterMenuItemPropertyChanged));

        public ListViewDetailControl()
        {
            InitializeComponent();
        }

        private static void OnMasterMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ListViewDetailControl;
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}
