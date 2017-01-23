using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Page_NS.Services;
using Page_NS.App_Page;

namespace Page_NS
{
    public partial class ViewModelLocator
    {
        public App_PageMasterDetailViewModel App_PageMasterDetailViewModel => ServiceLocator.Current.GetInstance<App_PageMasterDetailViewModel>();

        public void RegisterApp_Page(NavigationService navigationService)
        {
            SimpleIoc.Default.Register<App_PageMasterDetailViewModel>();
            navigationService.Configure(typeof(App_PageMasterDetailViewModel).FullName, typeof(App_PageMasterDetailPage));
        }
    }
}