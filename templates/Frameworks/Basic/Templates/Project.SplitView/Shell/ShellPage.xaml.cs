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

using uct.ItemName.Main;
using uct.ItemName.Services;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
namespace uct.ItemName.Shell
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShellPage : Page
    {
        public ShellPage()
        {
            ViewModel = new ShellViewModel();
            DataContext = ViewModel;
            this.InitializeComponent();

            NavigationService.SetNavigationFrame(frame);
        }

        public ShellViewModel ViewModel { get; private set; }

        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            var navigationItem = e.ClickedItem as ShellNavigationItem;
            if (navigationItem != null)
            {
                NavigationService.Navigate(navigationItem.PageType);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null || String.IsNullOrEmpty(e.Parameter.ToString()))
            {
                NavigationService.Navigate<MainPage>();
            }
            else
            {
                NavigationService.Navigate((Type)e.Parameter);
            }
        }

        private void frame_Navigated(object sender, NavigationEventArgs e)
        {
            var item = ViewModel.NavigationItems.FirstOrDefault(i => i.PageType == e.SourcePageType);
            if (item != null)
            {
                ViewModel.SelectedItem = item;
                return;
            }
        }
    }
}
