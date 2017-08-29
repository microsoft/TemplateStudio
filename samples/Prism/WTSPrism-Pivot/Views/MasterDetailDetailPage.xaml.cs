using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using WTSPrism.Models;
using WTSPrism.ViewModels;

namespace WTSPrism.Views
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
