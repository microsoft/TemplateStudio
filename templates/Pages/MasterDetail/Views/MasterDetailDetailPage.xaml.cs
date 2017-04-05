using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ItemNamespace.Models;

namespace ItemNamespace.Views
{
    public sealed partial class MasterDetailDetailPage : Page
    {
        public MasterDetailDetailPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.Item = e.Parameter as SampleModel;
        }
    }
}
