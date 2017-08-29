using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using WTSPrismNavigationBase.Models;
using WTSPrismNavigationBase.ViewModels;

namespace WTSPrismNavigationBase.Views
{
    public sealed partial class MasterDetailDetailPage : Page
    {
        private MasterDetailDetailPageViewModel ViewModel
        {
            get { return DataContext as MasterDetailDetailPageViewModel; }
        }

        public MasterDetailDetailPage()
        {
            InitializeComponent();
        }
    }
}
