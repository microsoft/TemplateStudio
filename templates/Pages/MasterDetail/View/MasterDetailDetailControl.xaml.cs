using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using ItemNamespace.Model;

namespace ItemNamespace.View
{
    public sealed partial class MasterDetailDetailControl : UserControl
    {
        public SampleModel Item
        {
            get { return GetValue(ItemProperty) as SampleModel; }
            set { SetValue(ItemProperty, value); }
        }
        public static DependencyProperty ItemProperty = DependencyProperty.Register("Item",typeof(SampleModel),typeof(MasterDetailDetailControl),new PropertyMetadata(null));

        public MasterDetailDetailControl()
        {
            this.InitializeComponent();
        }
    }
}
