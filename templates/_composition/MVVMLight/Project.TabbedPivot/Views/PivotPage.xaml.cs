using uct.ItemName.ViewModels;
using Windows.UI.Xaml.Controls;

namespace uct.ItemName.Views
{
    public sealed partial class PivotPage : Page
    {
        private PivotViewModel ViewModel { get { return DataContext as PivotViewModel; } }

        public PivotPage()
        {
            InitializeComponent();
        }
    }
}
