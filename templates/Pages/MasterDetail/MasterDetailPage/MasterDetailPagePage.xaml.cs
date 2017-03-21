using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ItemNamespace.MasterDetailPage
{
    public sealed partial class MasterDetailPagePage : Page
    {
        public MasterDetailPagePage()
        {
            this.InitializeComponent();
        }

        private void OnWindowStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            ViewModel.UpdateWindowState(e);
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadDataAsync(visualStateGroup.CurrentState);
        }
    }
}
