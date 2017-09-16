using Windows.UI.Xaml.Controls;
using WTSPrism.ViewModels;

namespace WTSPrism.Views
{
    public sealed partial class TabbedPage : Page
    {
        private TabbedPageViewModel ViewModel => DataContext as TabbedPageViewModel;

        public TabbedPage()
        {
            InitializeComponent();
        }
    }
}
