using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MahApps.Metro.Controls;
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Strings;

namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : System.ComponentModel.INotifyPropertyChanged, IDisposable
    {
        private readonly INavigationService _navigationService;
        private HamburgerMenuItem _selectedMenuItem;
        private System.Windows.Input.ICommand _goBackCommand;
        private ICommand _menuItemInvokedCommand;

        public HamburgerMenuItem SelectedMenuItem
        {
            get { return _selectedMenuItem; }
            set { Param_Setter(ref _selectedMenuItem, value); }
        }

        // TODO WTS: Change the icons and titles for all HamburgerMenuItems here.
        public ObservableCollection<HamburgerMenuItem> MenuItems { get; } = new ObservableCollection<HamburgerMenuItem>()
        {
        };

        public System.Windows.Input.ICommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new System.Windows.Input.ICommand(OnGoBack, CanGoBack));

        public ICommand MenuItemInvokedCommand => _menuItemInvokedCommand ?? (_menuItemInvokedCommand = new System.Windows.Input.ICommand(OnMenuItemInvoked));

        public ShellViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            _navigationService.Navigated += OnNavigated;
        }

        public void Dispose()
        {
            _navigationService.Navigated -= OnNavigated;
        }

        private bool CanGoBack()
            => _navigationService.CanGoBack;

        private void OnGoBack()
            => _navigationService.GoBack();

        private void OnMenuItemInvoked()
            => _navigationService.NavigateTo(SelectedMenuItem.TargetPageType.FullName);

        private void OnNavigated(object sender, string viewModelName)
        {
            var item = MenuItems
                        .OfType<HamburgerMenuItem>()
                        .FirstOrDefault(i => viewModelName == i.TargetPageType.FullName);
            if (item != null)
            {
                SelectedMenuItem = item;
            }

            GoBackCommand.Param_CanExecuteChangedMethodName();
        }
    }
}
