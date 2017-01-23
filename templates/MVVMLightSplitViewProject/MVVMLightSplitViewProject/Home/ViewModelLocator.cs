using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using MVVMLightSplitViewProject.Home;
using MVVMLightSplitViewProject.Services;

namespace MVVMLightSplitViewProject
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