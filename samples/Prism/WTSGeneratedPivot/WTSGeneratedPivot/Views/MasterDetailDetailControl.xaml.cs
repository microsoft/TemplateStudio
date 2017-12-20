using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using WTSGeneratedPivot.Models;

namespace WTSGeneratedPivot.Views
{
    public sealed partial class MasterDetailDetailControl : UserControl
    {
        public SampleOrder MasterMenuItem
        {
            get { return GetValue(MasterMenuItemProperty) as SampleOrder; }
            set { SetValue(MasterMenuItemProperty, value); }
        }

        public static readonly DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem", typeof(SampleOrder), typeof(MasterDetailDetailControl), new PropertyMetadata(null));

        public MasterDetailDetailControl()
        {
            InitializeComponent();
        }
    }
}
