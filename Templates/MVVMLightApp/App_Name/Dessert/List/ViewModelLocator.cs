using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using App_Name.Dessert.List;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Name
{
    public partial class ViewModelLocator
    {
        public DessertListViewModel DessertListViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DessertListViewModel>();
            }
        }

        private void RegisterDessertList(NavigationService navigationService)
        {
            SimpleIoc.Default.Register<DessertListViewModel>();
            navigationService.Configure(typeof(DessertListViewModel).FullName, typeof(DessertListPage));
        }
    }
}