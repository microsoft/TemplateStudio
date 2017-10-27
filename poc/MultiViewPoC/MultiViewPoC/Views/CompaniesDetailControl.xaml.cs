using System;

using MultiViewPoC.Models;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MultiViewPoC.Views
{
    public sealed partial class CompaniesDetailControl : UserControl
    {
        public SampleOrder MasterMenuItem
        {
            get { return GetValue(MasterMenuItemProperty) as SampleOrder; }
            set { SetValue(MasterMenuItemProperty, value); }
        }

        public static readonly DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem", typeof(SampleOrder), typeof(CompaniesDetailControl), new PropertyMetadata(null, OnMasterMenuItemPropertyChanged));

        public CompaniesDetailControl()
        {
            InitializeComponent();
        }

        private static void OnMasterMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as CompaniesDetailControl;
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}
