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
using SplitViewProject.Home;
#if (isMVVMLight)
using Microsoft.Practices.ServiceLocation;
using SplitViewProject.Services;
#endif

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SplitViewProject.Shell
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShellPage : Page
    {
        public ShellPage()
        {
            this.InitializeComponent();
#if (isBasic)
            ViewModel = new ShellViewModel();
            DataContext = ViewModel;
            App.NavigationService.SetNavigationFrame(frame);
#else if (isMVVMLight)
            var navigationService = ServiceLocator.Current.GetInstance<NavigationService>();
            navigationService.SetNavigationFrame(frame);
#endif
        }
#if (isBasic)
        public ShellViewModel ViewModel { get; private set; }
#endif

        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            var navigationItem = e.ClickedItem as ShellNavigationItem;
            if (navigationItem != null)
            {
#if (isBasic)
                App.NavigationService.Navigate(navigationItem.PageType);
#else if (isMVVMLight)
                var navigationService = ServiceLocator.Current.GetInstance<NavigationService>();
                navigationService.Navigate(navigationItem.ViewModelName);
#endif
            }
        }

        private void OnNavigationItemsLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ListView paneitems = sender as ListView;
            if (paneitems.Items?.Count > 0)
            {
                paneitems.SelectedIndex = 0;
#if (isBasic)
                App.NavigationService.Navigate<HomePage>();
#else if (isMVVMLight)
                var navigationService = ServiceLocator.Current.GetInstance<NavigationService>();
                navigationService.Navigate(typeof(HomeViewModel).FullName);
#endif
            }
        }
    }
}
