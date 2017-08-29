using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using WTSPrism.ViewModels;

namespace WTSPrism.Views
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
