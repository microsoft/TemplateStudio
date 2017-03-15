using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using uct.ItemName.Services;

namespace uct.ItemName.Shell
{
    public sealed partial class ShellPage : Page
    {
        public ShellViewModel ViewModel { get; } = new ShellViewModel();

        public ShellPage()
        {
            DataContext = ViewModel;
            this.InitializeComponent();

            NavigationService.SetNavigationFrame(frame);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) => ViewModel.Initialize(e);

        private void OnFrameNavigated(object sender, NavigationEventArgs e) => ViewModel.SetNavigationItem(e);
    }
}
