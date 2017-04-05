using uct.ItemName.ViewModels;
using Windows.UI.Xaml.Controls;

namespace uct.ItemName.Views
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
