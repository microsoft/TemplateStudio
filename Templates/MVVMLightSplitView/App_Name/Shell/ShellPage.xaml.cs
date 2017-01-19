using App_Name.Home;
using App_Name.Services;
using Microsoft.Practices.ServiceLocation;
using Windows.UI.Xaml.Controls;

namespace App_Name.Shell
{
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
