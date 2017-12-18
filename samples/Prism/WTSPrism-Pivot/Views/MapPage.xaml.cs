using Windows.UI.Xaml.Controls;
using WTSPrism.ViewModels;

namespace WTSPrism.Views
{
    public sealed partial class MapPage : Page
    {
        private MapPageViewModel ViewModel => DataContext as MapPageViewModel;

        public MapPage()
        {
            InitializeComponent();
        }
    }
}
