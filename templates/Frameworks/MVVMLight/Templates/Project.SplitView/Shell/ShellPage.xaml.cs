using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.ServiceLocation;
using uct.ItemName.Services;

namespace uct.ItemName.Shell
{
    public sealed partial class ShellPage : Page
    {
        private ShellViewModel ViewModel
        {
            get { return DataContext as ShellViewModel; }
        }

        private NavigationService navigationService
        {
            get { return ServiceLocator.Current.GetInstance<NavigationService>(); }
        }

        public ShellPage()
        {
            this.InitializeComponent();

            navigationService.SetNavigationFrame(frame);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) => ViewModel.Initialize(e);

        private void OnFrameNavigated(object sender, NavigationEventArgs e) => ViewModel.SetNavigationItem(e);
    }
}
