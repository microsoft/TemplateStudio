using System;
using System.Linq;
using System.Windows.Input;

using AdvancedNavigationPaneProject.Helpers;
using AdvancedNavigationPaneProject.Services;
using AdvancedNavigationPaneProject.Views;

using Windows.UI.Xaml.Controls;

namespace AdvancedNavigationPaneProject.ViewModels
{
    public class SecondShellViewModel : Observable
    {
        private NavigationView _navigationView;
        private NavigationViewItem _selected;
        private ICommand _itemInvokedCommand;
        private ICommand _webSiteCommand;

        public NavigationViewItem Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public ICommand ItemInvokedCommand => _itemInvokedCommand ?? (_itemInvokedCommand = new RelayCommand<NavigationViewItemInvokedEventArgs>(OnItemInvoked));

        public ICommand WebSiteCommand => _webSiteCommand ?? (_webSiteCommand = new RelayCommand(OnWebSite));        

        public SecondShellViewModel()
        {
        }

        public void Initialize(Frame frame, NavigationView navigationView)
        {
            NavigationService.InitializeFrame("Third", frame);
            _navigationView = navigationView;
            NavigationService.Navigated += OnNavigated;
        }

        private void OnItemInvoked(NavigationViewItemInvokedEventArgs args)
        {
            var item = _navigationView.MenuItems
                            .OfType<NavigationViewItem>()
                            .First(menuItem => (string)menuItem.Content == (string)args.InvokedItem);
            var pageType = item.GetValue(NavHelper.NavigateToProperty) as Type;
            NavigationService.Navigate(pageType, new NavigateConfig("Third"));
        }

        private void OnNavigated(object sender, NavigationEventArgsEx e)
        {
            if (e.FrameKey == "Third")
            {
                Selected = _navigationView.MenuItems
                                .OfType<NavigationViewItem>()
                                .FirstOrDefault(menuItem => IsMenuItemForPageType(menuItem, e.SourcePageType));
            }
        }

        private bool IsMenuItemForPageType(NavigationViewItem menuItem, Type sourcePageType)
        {
            var pageType = menuItem.GetValue(NavHelper.NavigateToProperty) as Type;
            return pageType == sourcePageType;
        }

        private void OnWebSite()
        {
            NavigationService.Navigate<WebSitePage>();
        }
    }
}
