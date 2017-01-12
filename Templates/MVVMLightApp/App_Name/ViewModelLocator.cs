using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;

using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace App_Name
{
    public partial class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            var navigationService = new NavigationService();
            SimpleIoc.Default.Register<INavigationService>(() => navigationService);

            RegisterDessertList(navigationService);
            RegisterDessertDetail(navigationService);
            //G3N: REGISTER VIEW_MODEL
        }

        private async Task NotifyUserMethod(NotificationMessage message)
        {
            var msg = new MessageDialog(message.Notification);
            await msg.ShowAsync();
        }
    }
}
