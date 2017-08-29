using Windows.UI.Xaml.Controls;

using WTSPrism.ViewModels;

namespace WTSPrism.Views
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
