using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Param_ItemNamespace.Models;

namespace Param_ItemNamespace.Views
{
    public sealed partial class MasterDetailDetailControl : UserControl
    {
        public Order MasterMenuItem
        {
            get { return GetValue(MasterMenuItemProperty) as Order; }
            set { SetValue(MasterMenuItemProperty, value); }
        }

        public static readonly DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem", typeof(Order), typeof(MasterDetailDetailControl), new PropertyMetadata(null));

        public MasterDetailDetailControl()
        {
            InitializeComponent();
        }
    }
}
