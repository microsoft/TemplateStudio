using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using App_Name.Dessert.Detail;
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
        public DessertDetailViewModel DessertDetailViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DessertDetailViewModel>();
            }
        }

        private void RegisterDessertDetail(NavigationService navigationService)
        {
            SimpleIoc.Default.Register<DessertDetailViewModel>(true);
            navigationService.Configure(typeof(DessertDetailViewModel).FullName, typeof(DessertDetailPage));
        }
    }
}