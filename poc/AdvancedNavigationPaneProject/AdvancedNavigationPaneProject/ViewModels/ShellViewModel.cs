using System;
using System.Linq;
using System.Windows.Input;

using AdvancedNavigationPaneProject.Helpers;
using AdvancedNavigationPaneProject.Services;
using AdvancedNavigationPaneProject.Views;

using Windows.UI.Xaml.Controls;

namespace AdvancedNavigationPaneProject.ViewModels
{
    public class ShellViewModel : Observable
    {
        private NavigationView _navigationView;
        private NavigationViewItem _selected;
        private ICommand _itemInvokedCommand;
        private ICommand _secondShellCommand;
        private ICommand _webSiteCommand;
        private ICommand _logOutCommand;

        public NavigationViewItem Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public ICommand ItemInvokedCommand => _itemInvokedCommand ?? (_itemInvokedCommand = new RelayCommand<NavigationViewItemInvokedEventArgs>(OnItemInvoked));

        public ICommand SecondShellCommand => _secondShellCommand ?? (_secondShellCommand = new RelayCommand(OnSecondShell));

        public ICommand WebSiteCommand => _webSiteCommand ?? (_webSiteCommand = new RelayCommand(OnWebSite));

        public ICommand LogOutCommand => _logOutCommand ?? (_logOutCommand = new RelayCommand(OnLogOut));

        public ShellViewModel()
        {
        }

        public void Initialize(Frame frame, NavigationView navigationView)
        {
            _navigationView = navigationView;
            NavigationService.InitializeFrame("Secondary", frame);
            NavigationService.Navigated += OnNavigated;
        }

        private void OnItemInvoked(NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                NavigationService.Navigate<SettingsPage>(new NavigateConfig("Secondary"));
                return;
            }

            var item = _navigationView.MenuItems
                            .OfType<NavigationViewItem>()
                            .First(menuItem => (string)menuItem.Content == (string)args.InvokedItem);
            var pageType = item.GetValue(NavHelper.NavigateToProperty) as Type;
            NavigationService.Navigate(pageType, new NavigateConfig("Secondary"));
        }

        private void OnNavigated(object sender, NavigationEventArgsEx e)
        {
            if (e.FrameKey == "Secondary")
            {
                if (e.SourcePageType == typeof(SettingsPage))
                {
                    Selected = _navigationView.SettingsItem as NavigationViewItem;
                    return;
                }

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

        private void OnSecondShell()
        {
            NavigationService.Navigate<SecondShellPage>(new NavigateConfig("Main"));
            NavigationService.Navigate<SecondMainPage>(new NavigateConfig("Third"));
        }

        private void OnWebSite()
        {
            NavigationService.Navigate<WebSitePage>();
        }

        private void OnLogOut()
        {
            NavigationService.RestartNavigation();
            NavigationService.Navigate<StartUpPage>();
        }
    }
}
