using wts.ItemName.ViewModels;
using Windows.UI.Xaml.Controls;

namespace wts.ItemName.Views
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
