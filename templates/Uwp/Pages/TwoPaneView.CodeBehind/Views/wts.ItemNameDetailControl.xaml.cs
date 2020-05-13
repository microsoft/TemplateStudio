using Param_RootNamespace.Core.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Param_RootNamespace.Views
{
    public sealed partial class wts.ItemNameDetailControl : UserControl
    {
        public SampleOrder SelectedItem
        {
            get { return GetValue(SelectedItemProperty) as SampleOrder; }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof(SelectedItem), typeof(SampleOrder), typeof(wts.ItemNameDetailControl), new PropertyMetadata(null, OnSelectedItemPropertyChanged));

        public wts.ItemNameDetailControl()
        {
            InitializeComponent();
        }

        private static void OnSelectedItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as wts.ItemNameDetailControl;
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}
