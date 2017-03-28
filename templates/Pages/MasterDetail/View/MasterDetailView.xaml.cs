using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ItemNamespace.View
{
    public sealed partial class MasterDetailView : Page
    {
        public MasterDetailView()
        {
            this.InitializeComponent();
        }

        private void OnWindowStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            ViewModel.UpdateWindowState(e);
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.LoadDataAsync(visualStateGroup.CurrentState);
        }
    }
}
