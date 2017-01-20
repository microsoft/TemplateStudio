using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Page_NS.Services;
using Page_NS.App_Page;

namespace Page_NS
{
    public partial class ViewModelLocator
    {
        public App_PageViewModel App_PageViewModel => ServiceLocator.Current.GetInstance<App_PageViewModel>();

        public void RegisterApp_Page(NavigationService navigationService)
        {
            SimpleIoc.Default.Register<App_PageViewModel>();
            navigationService.Configure(typeof(App_PageViewModel).FullName, typeof(App_PagePage));
        }
    }
}