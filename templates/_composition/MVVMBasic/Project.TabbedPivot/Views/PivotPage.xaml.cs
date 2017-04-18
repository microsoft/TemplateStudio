using wts.ItemName.ViewModels;
using Windows.UI.Xaml.Controls;

namespace wts.ItemName.Views
{
    public sealed partial class PivotPage : Page
    {
        public PivotViewModel ViewModel { get; } = new PivotViewModel();

        public PivotPage()
        {
            DataContext = ViewModel;
            InitializeComponent();
        }
    }
}
