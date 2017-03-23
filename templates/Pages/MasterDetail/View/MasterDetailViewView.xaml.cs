using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ItemNamespace.View
{
    public sealed partial class MasterDetailViewView : Page
    {
        public MasterDetailViewView()
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
