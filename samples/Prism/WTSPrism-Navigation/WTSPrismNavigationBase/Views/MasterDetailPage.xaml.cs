using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using WTSPrismNavigationBase.ViewModels;

namespace WTSPrismNavigationBase.Views
{
    public sealed partial class MasterDetailPage : Page
    {
        private MasterDetailPageViewModel ViewModel
        {
            get { return DataContext as MasterDetailPageViewModel; }
        }

        public MasterDetailPage()
        {
            InitializeComponent();
        }
    }
}
