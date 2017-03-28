using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using ItemNamespace.Model;
using ItemNamespace.Services;

namespace ItemNamespace.View
{
    public sealed partial class MasterDetailDetailView : Page
    {
        public MasterDetailDetailView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.Item = e.Parameter as SampleModel;
        }

        private void OnWindowStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            ViewModel.UpdateWindowState(e);
        }
    }
}
