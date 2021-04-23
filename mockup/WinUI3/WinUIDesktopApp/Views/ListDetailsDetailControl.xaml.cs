using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using WinUIDesktopApp.Core.Models;

namespace WinUIDesktopApp.Views
{
    public sealed partial class ListDetailsDetailControl : UserControl
    {
        public SampleOrder ListDetailsMenuItem
        {
            get { return GetValue(ListDetailsMenuItemProperty) as SampleOrder; }
            set { SetValue(ListDetailsMenuItemProperty, value); }
        }

        public static readonly DependencyProperty ListDetailsMenuItemProperty = DependencyProperty.Register("ListDetailsMenuItem", typeof(SampleOrder), typeof(ListDetailsDetailControl), new PropertyMetadata(null, OnListDetailsMenuItemPropertyChanged));

        public ListDetailsDetailControl()
        {
            InitializeComponent();
        }

        private static void OnListDetailsMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ListDetailsDetailControl;
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}
