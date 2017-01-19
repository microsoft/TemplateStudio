using Caliburn.Micro;
using App_Name.Dessert.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace App_Name.Shell
{
    public class ShellViewModel
    {
        private readonly WinRTContainer _container;
        private readonly IEventAggregator _eventAggregator;

        public ShellViewModel(WinRTContainer container, IEventAggregator eventAggregator)
        {
            _container = container;
            _eventAggregator = eventAggregator;
        }

        public void SetupNavigationService(Frame frame)
        {
            if (frame != null)
            {
                if (_container.HasHandler(typeof(INavigationService), null))
                    _container.UnregisterHandler(typeof(INavigationService), null);

                var navigationService = _container.RegisterNavigationService(frame);

                ConfigureNavigationBar(navigationService);

                navigationService.NavigateToViewModel<DessertListViewModel>();
            }
        }

        private static void ConfigureNavigationBar(INavigationService navigationService)
        {
            if (SystemNavigationManager.GetForCurrentView() != null)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested += ((sender, e) =>
                {
                    if (navigationService.CanGoBack)
                    {
                        navigationService.GoBack();
                        e.Handled = true;
                    }
                });

                navigationService.Navigated += ((sender, e) =>
                {
                    if (navigationService.CanGoBack)
                    {
                        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                    }
                    else
                    {
                        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                    }
                });
            }
        }
    }
}
