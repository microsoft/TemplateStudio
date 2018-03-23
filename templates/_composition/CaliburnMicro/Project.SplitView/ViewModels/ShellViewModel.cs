using System;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Caliburn.Micro;
using wts.ItemName.Helpers;
using wts.ItemName.Views;

namespace wts.ItemName.ViewModels
{
    public class ShellViewModel : Screen
    {
        private readonly WinRTContainer _container;
        private INavigationService _navigationService;
        private NavigationView _navigationView;
        private object _selected;

        public ShellViewModel(WinRTContainer container)
        {
            _container = container;
        }

        public object Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        protected override void OnInitialize()
        {
            var view = GetView() as IShellView;

            _navigationService = view?.CreateNavigationService(_container);
            _navigationView = view?.GetNavigationView();

            if (_navigationService != null)
            {
                _navigationService.Navigated += NavigationService_Navigated;
            }
        }

        private void OnItemInvoked(NavigationViewItemInvokedEventArgs args)
        {
            var item = _navigationView.MenuItems
                            .OfType<NavigationViewItem>()
                            .First(menuItem => (string)menuItem.Content == (string)args.InvokedItem);
            var pageType = item.GetValue(NavigationViewItemExtensions.PageTypeProperty) as Type;
            var viewModelType = ViewModelLocator.LocateTypeForViewType(pageType, false);
            _navigationService.NavigateToViewModel(viewModelType);
        }

        private void NavigationService_Navigated(object sender, NavigationEventArgs e)
        {
           Selected = _navigationView.MenuItems
                            .OfType<NavigationViewItem>()
                            .First(menuItem => IsNavigationViewItemFromPageType(menuItem, e.SourcePageType));
        }

        private bool IsNavigationViewItemFromPageType(NavigationViewItem menuItem, Type sourcePageType)
        {
            var sourceViewModelType = ViewModelLocator.LocateTypeForViewType(sourcePageType, false);
            var pageType = menuItem.GetValue(NavigationViewItemExtensions.PageTypeProperty) as Type;
            var viewModelType = ViewModelLocator.LocateTypeForViewType(pageType, false);
            return viewModelType == sourceViewModelType;
        }
    }
}
