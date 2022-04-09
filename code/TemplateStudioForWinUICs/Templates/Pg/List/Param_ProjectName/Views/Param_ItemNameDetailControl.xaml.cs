using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Param_RootNamespace.Core.Models;

namespace Param_RootNamespace.Views
{
    public sealed partial class Param_ItemNameDetailControl : UserControl
    {
        public SampleOrder ListDetailsMenuItem
        {
            get { return GetValue(ListDetailsMenuItemProperty) as SampleOrder; }
            set { SetValue(ListDetailsMenuItemProperty, value); }
        }

        public static readonly DependencyProperty ListDetailsMenuItemProperty = DependencyProperty.Register("ListDetailsMenuItem", typeof(SampleOrder), typeof(Param_ItemNameDetailControl), new PropertyMetadata(null, OnListDetailsMenuItemPropertyChanged));

        public Param_ItemNameDetailControl()
        {
            InitializeComponent();
        }

        private static void OnListDetailsMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Param_ItemNameDetailControl;
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}
