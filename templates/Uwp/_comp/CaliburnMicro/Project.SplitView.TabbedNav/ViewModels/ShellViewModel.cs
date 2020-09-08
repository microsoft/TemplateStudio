using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using WinUI = Microsoft.UI.Xaml.Controls;
using Caliburn.Micro;
using Param_RootNamespace.Helpers;
using Param_RootNamespace.Views;

namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : Screen
    {
        private readonly KeyboardAccelerator _altLeftKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu);
        private readonly KeyboardAccelerator _backKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.GoBack);

        private readonly WinRTContainer _container;
        private static INavigationService _navigationService;
        private WinUI.NavigationView _navigationView;
        private bool _isBackEnabled;
        private WinUI.NavigationViewItem _selected;

        public ShellViewModel(WinRTContainer container)
        {
            _container = container;
        }

        public bool IsBackEnabled
        {
            get { return _isBackEnabled; }
            set { Set(ref _isBackEnabled, value); }
        }

        public WinUI.NavigationViewItem Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            var view = GetView() as IShellView;

            _navigationService = view?.CreateNavigationService(_container);
            _navigationView = view?.GetNavigationView();

            if (_navigationService != null)
            {
                _navigationService.NavigationFailed += (sender, e) =>
                {
                    throw e.Exception;
                };
                _navigationService.Navigated += NavigationService_Navigated;
                _navigationView.BackRequested += OnBackRequested;
            }
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            if (GetView() is UIElement page)
            {
                page.KeyboardAccelerators.Add(_altLeftKeyboardAccelerator);
                page.KeyboardAccelerators.Add(_backKeyboardAccelerator);
            }
        }

        private void OnItemInvoked(WinUI.NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                // Navigate to the settings page - implement as appropriate if needed
            }
            else if (args.InvokedItemContainer is WinUI.NavigationViewItem selectedItem)
            {
                var pageType = selectedItem.GetValue(NavHelper.NavigateToProperty) as Type;
                var viewModelType = ViewModelLocator.LocateTypeForViewType(pageType, false);
                _navigationService.NavigateToViewModel(viewModelType);
            }
        }

        private void OnBackRequested(WinUI.NavigationView sender, WinUI.NavigationViewBackRequestedEventArgs args)
        {
            _navigationService.GoBack();
        }

        private void NavigationService_Navigated(object sender, NavigationEventArgs e)
        {
            IsBackEnabled = _navigationService.CanGoBack;
            var selectedItem = GetSelectedItem(_navigationView.MenuItems, e.SourcePageType);
            if (selectedItem != null)
            {
                Selected = selectedItem;
            }
        }

        private WinUI.NavigationViewItem GetSelectedItem(IEnumerable<object> menuItems, Type pageType)
        {
            foreach (var item in menuItems.OfType<WinUI.NavigationViewItem>())
            {
                if (IsMenuItemForPageType(item, pageType))
                {
                    return item;
                }

                var selectedChild = GetSelectedItem(item.MenuItems, pageType);
                if (selectedChild != null)
                {
                    return selectedChild;
                }
            }

            return null;
        }

        private bool IsMenuItemForPageType(WinUI.NavigationViewItem menuItem, Type sourcePageType)
        {
            var sourceViewModelType = ViewModelLocator.LocateTypeForViewType(sourcePageType, false);
            var pageType = menuItem.GetValue(NavHelper.NavigateToProperty) as Type;
            var viewModelType = ViewModelLocator.LocateTypeForViewType(pageType, false);
            return viewModelType == sourceViewModelType;
        }

        private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
        {
            var keyboardAccelerator = new KeyboardAccelerator() { Key = key };
            if (modifiers.HasValue)
            {
                keyboardAccelerator.Modifiers = modifiers.Value;
            }

            keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;
            return keyboardAccelerator;
        }

        private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (_navigationService.CanGoBack)
            {
                _navigationService.GoBack();
                args.Handled = true;
            }
        }
    }
}
