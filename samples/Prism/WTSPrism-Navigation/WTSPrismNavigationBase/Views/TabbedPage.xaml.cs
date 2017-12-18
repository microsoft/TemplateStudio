using Windows.UI.Xaml.Controls;

using WTSPrismNavigationBase.ViewModels;

namespace WTSPrismNavigationBase.Views
{
    public sealed partial class TabbedPage : Page
    {
        private TabbedPageViewModel ViewModel
        {
            get { return DataContext as TabbedPageViewModel; }
        }

        public TabbedPage()
        {
            InitializeComponent();
        }
    }
}
