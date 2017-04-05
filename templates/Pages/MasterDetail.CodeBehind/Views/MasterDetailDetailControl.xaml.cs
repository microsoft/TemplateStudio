using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ItemNamespace.Models;

namespace ItemNamespace.Views
{
    public sealed partial class MasterDetailDetailControl : UserControl
    {
        public SampleModel MasterMenuItem
        {
            get { return GetValue(MasterMenuItemProperty) as SampleModel; }
            set { SetValue(MasterMenuItemProperty, value); }
        }
        public static DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem",typeof(SampleModel),typeof(MasterDetailDetailControl),new PropertyMetadata(null));

        public MasterDetailDetailControl()
        {
            InitializeComponent();
        }
    }
}
