using wts.ItemName.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace wts.ItemName.Views
{
    public sealed partial class PivotPage : Page
    {
        private PivotViewModel ViewModel { get { return DataContext as PivotViewModel; } }

        public PivotPage()
        {
            NavigationCacheMode = NavigationCacheMode.Required;
            InitializeComponent();
        }
    }
}
