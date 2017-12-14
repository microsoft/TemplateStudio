using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using WTSPrismNavigationBase.ViewModels;

namespace WTSPrismNavigationBase.Views
{
    public sealed partial class MapPage : Page
    {
        private MapPageViewModel ViewModel
        {
            get { return DataContext as MapPageViewModel; }
        }

        public MapPage()
        {
            InitializeComponent();
        }
    }
}
