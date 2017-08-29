using Windows.UI.Xaml.Controls;

using WTSPrism.ViewModels;

namespace WTSPrism.Views
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
