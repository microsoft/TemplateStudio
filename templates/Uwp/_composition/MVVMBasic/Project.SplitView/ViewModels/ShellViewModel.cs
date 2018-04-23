using System;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using wts.ItemName.Services;
using wts.ItemName.Views;
using wts.ItemName.Helpers;

namespace wts.ItemName.ViewModels
{
    public class ShellViewModel : Observable
    {
        private NavigationView _navigationView;
        private NavigationViewItem _selected;
        private ICommand _itemInvokedCommand;

        public NavigationViewItem Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public ICommand ItemInvokedCommand => _itemInvokedCommand ?? (_itemInvokedCommand = new RelayCommand<NavigationViewItemInvokedEventArgs>(OnItemInvoked));

        public ShellViewModel()
        {
        }

        public void Initialize(Frame frame, NavigationView navigationView)
        {
            _navigationView = navigationView;
            NavigationService.Frame = frame;
            NavigationService.Navigated += Frame_Navigated;
        }

        private void OnItemInvoked(NavigationViewItemInvokedEventArgs args)
        {
            var item = _navigationView.MenuItems
                            .OfType<NavigationViewItem>()
                            .First(menuItem => (string)menuItem.Content == (string)args.InvokedItem);
            var pageType = item.GetValue(NavHelper.NavigateToProperty) as Type;
            NavigationService.Navigate(pageType);
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            Selected = _navigationView.MenuItems
                            .OfType<NavigationViewItem>()
                            .FirstOrDefault(menuItem => IsMenuItemForPageType(menuItem, e.SourcePageType));
        }

        private bool IsMenuItemForPageType(NavigationViewItem menuItem, Type sourcePageType)
        {
            var pageType = menuItem.GetValue(NavHelper.NavigateToProperty) as Type;
            return pageType == sourcePageType;
        }
    }
}
