using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using App_Name.App_Page;

namespace App_Name
{
    public partial class ViewModelLocator
    {
        public App_PageViewModel App_PageViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<App_PageViewModel>();
            }
        }

        private void RegisterApp_Page(NavigationService navigationService)
        {
            SimpleIoc.Default.Register<App_PageViewModel>();
            navigationService.Configure(typeof(App_PageViewModel).FullName, typeof(App_PagePage));
        }
    }
}