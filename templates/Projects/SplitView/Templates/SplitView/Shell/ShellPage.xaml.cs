using uct.SplitViewProject.Services;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace uct.SplitViewProject.Shell
{
    public sealed partial class ShellPage : Page
    {
        public ShellPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) => ViewModel.Initialize(e);

        private void OnFrameNavigated(object sender, NavigationEventArgs e) => ViewModel.SetNavigationItem(e);
    }
}
