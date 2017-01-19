using SplitViewTemplate.Model;
using SplitViewTemplate.ViewModel;
using Windows.UI.Xaml.Controls;

namespace SplitViewTemplate
{
    public sealed partial class Shell : Page
    {
        ShellViewModel ViewModel => (ShellViewModel)DataContext;
        public Shell()
        {
            this.InitializeComponent();
            ViewModel.SetNavigationFrame(frame);            
        }

        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            var navigationItem = e.ClickedItem as ShellNavigationItem;
            if (navigationItem != null)
            {
                ViewModel.Navigate(navigationItem.PageName);
            }
        }

        private void OnNavigationItemsLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ListView paneitems = sender as ListView;
            if (paneitems.Items?.Count > 0)
            {
                paneitems.SelectedIndex = 0;
                ViewModel.Navigate(ViewModelLocator.HomeViewKey);
            }
        }
    }
}
