using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
    }
}
