using Windows.UI.Xaml.Controls;
using WTSPrism.ViewModels;

namespace WTSPrism.Views
{
    public sealed partial class MasterDetailPage : Page
    {
        private MasterDetailPageViewModel ViewModel => DataContext as MasterDetailPageViewModel;

        public MasterDetailPage()
        {
            InitializeComponent();
        }
    }
}
