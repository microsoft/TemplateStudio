using App_Name.Home;
using App_Name.Services;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace App_Name
{
    public partial class ViewModelLocator
    {
        public HomeViewModel HomeViewModel => ServiceLocator.Current.GetInstance<HomeViewModel>();

        public void RegisterHome(NavigationService navigationService)
        {
            SimpleIoc.Default.Register<HomeViewModel>();
            navigationService.Configure(typeof(HomeViewModel).FullName, typeof(HomePage));
        }
    }
}
