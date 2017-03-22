using uct.SplitViewProject.Services;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace uct.SplitViewProject.View
{
    public sealed partial class ShellView : Page
    {
        public ShellView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) => ViewModel.Initialize(e);

        private void OnFrameNavigated(object sender, NavigationEventArgs e) => ViewModel.SetNavigationItem(e);
    }
}
