using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using WtsXamarinUWP.UWP.Models;

namespace WtsXamarinUWP.UWP.Views
{
    public sealed partial class ListViewDetailControl : UserControl
    {
        public SampleOrderWithSymbol MasterMenuItem
        {
            get { return GetValue(MasterMenuItemProperty) as SampleOrderWithSymbol; }
            set { SetValue(MasterMenuItemProperty, value); }
        }

        public static readonly DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem", typeof(SampleOrderWithSymbol), typeof(ListViewDetailControl), new PropertyMetadata(null, OnMasterMenuItemPropertyChanged));

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
