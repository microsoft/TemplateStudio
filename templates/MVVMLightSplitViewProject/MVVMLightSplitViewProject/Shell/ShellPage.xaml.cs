using Microsoft.Practices.ServiceLocation;
using MVVMLightSplitViewProject.Home;
using MVVMLightSplitViewProject.Services;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MVVMLightSplitViewProject.Shell
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShellPage : Page
    {
        public ShellPage()
        {
            this.InitializeComponent();
            var navigationService = ServiceLocator.Current.GetInstance<NavigationService>();
            navigationService.SetNavigationFrame(frame);
        }

        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            var navigationItem = e.ClickedItem as ShellNavigationItem;
            if (navigationItem != null)
            {
                var navigationService = ServiceLocator.Current.GetInstance<NavigationService>();
                navigationService.Navigate(navigationItem.ViewModelName);
            }
        }

        private void OnNavigationItemsLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ListView paneitems = sender as ListView;
            if (paneitems.Items?.Count > 0)
            {
                paneitems.SelectedIndex = 0;
                var navigationService = ServiceLocator.Current.GetInstance<NavigationService>();
                navigationService.Navigate(typeof(HomeViewModel).FullName);
            }
        }
    }
}
