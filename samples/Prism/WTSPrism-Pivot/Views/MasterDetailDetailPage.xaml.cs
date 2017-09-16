using Windows.UI.Xaml.Controls;
using WTSPrism.ViewModels;

namespace WTSPrism.Views
{
    public sealed partial class MasterDetailDetailPage : Page
    {
        private MasterDetailDetailPageViewModel ViewModel => DataContext as MasterDetailDetailPageViewModel;

        public MasterDetailDetailPage()
        {
            InitializeComponent();
        }
    }
}
