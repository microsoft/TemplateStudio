using Windows.UI.Xaml.Controls;

using WTSPrismNavigationBase.ViewModels;

namespace WTSPrismNavigationBase.Views
{
    public sealed partial class MainPage : Page
    {
        private MainPageViewModel ViewModel
        {
            get { return DataContext as MainPageViewModel; }
        }

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
