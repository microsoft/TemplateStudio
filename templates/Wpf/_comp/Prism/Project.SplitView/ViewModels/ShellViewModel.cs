using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MahApps.Metro.Controls;
using Param_RootNamespace.Constants;
using Param_RootNamespace.Models;
using Param_RootNamespace.Strings;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private IRegionNavigationService _navigationService;
        private HamburgerMenuItem _selectedMenuItem;
        private DelegateCommand _goBackCommand;
        private ICommand _loadedCommand;
        private ICommand _menuItemInvokedCommand;

        public HamburgerMenuItem SelectedMenuItem
        {
            get { return _selectedMenuItem; }
            set { SetProperty(ref _selectedMenuItem, value); }
        }

        // TODO WTS: Change the icons and titles for all HamburgerMenuItems here.
        public ObservableCollection<HamburgerMenuItem> MenuItems { get; } = new ObservableCollection<HamburgerMenuItem>()
        {
        };

        public DelegateCommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new DelegateCommand(OnGoBack, CanGoBack));

        public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new DelegateCommand(OnLoaded));

        public ICommand MenuItemInvokedCommand => _menuItemInvokedCommand ?? (_menuItemInvokedCommand = new DelegateCommand(OnMenuItemInvoked));

        public ShellViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        private void OnLoaded()
        {
            _navigationService = _regionManager.Regions[Regions.Main].NavigationService;
            _navigationService.Navigated += OnNavigated;
            SelectedMenuItem = MenuItems.First();
        }

        private bool CanGoBack()
            => _navigationService != null && _navigationService.Journal.CanGoBack;

        private void OnGoBack()
            => _navigationService.Journal.GoBack();

        private void OnMenuItemInvoked()
            => RequestNavigate(SelectedMenuItem.Tag.ToString());

        private void RequestNavigate(string target)
        {
            if (_navigationService.CanNavigate(target))
            {
                _navigationService.RequestNavigate(target);
            }
        }

        private void OnNavigated(object sender, RegionNavigationEventArgs e)
        {
            var item = MenuItems
                        .OfType<HamburgerMenuItem>()
                        .FirstOrDefault(i => e.Uri.ToString() == i.Tag.ToString());
            if (item != null)
            {
                SelectedMenuItem = item;
            }

            GoBackCommand.RaiseCanExecuteChanged();
        }
    }
}
