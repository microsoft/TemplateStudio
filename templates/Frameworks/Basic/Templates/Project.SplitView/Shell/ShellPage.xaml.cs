using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using ItemName.Home;
using ItemName.Services;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
namespace ItemName.Shell
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShellPage : Page
    {
        public ShellPage()
        {
            this.InitializeComponent();

            ViewModel = new ShellViewModel();
            DataContext = ViewModel;
            NavigationService.Instance.SetNavigationFrame(frame);
        }

        public ShellViewModel ViewModel { get; private set; }

        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            var navigationItem = e.ClickedItem as ShellNavigationItem;
            if (navigationItem != null)
            {
                NavigationService.Instance.Navigate(navigationItem.PageType);
            }
        }

        private void OnNavigationItemsLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ListView paneitems = sender as ListView;
            if (paneitems.Items?.Count > 0)
            {
                paneitems.SelectedIndex = 0;

                NavigationService.Instance.Navigate<HomePage>();
            }
        }
    }
}
