using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Page_NS.Services;
using Page_NS.MVVMLightMasterDetailPage;

namespace Page_NS
{
    public partial class ViewModelLocator
    {
        public MVVMLightMasterDetailPageViewModel MVVMLightMasterDetailPageViewModel => ServiceLocator.Current.GetInstance<MVVMLightMasterDetailPageViewModel>();

        public void RegisterMVVMLightMasterDetailPage(NavigationService navigationService)
        {
            SimpleIoc.Default.Register<MVVMLightMasterDetailPageViewModel>();
            navigationService.Configure(typeof(MVVMLightMasterDetailPageViewModel).FullName, typeof(MVVMLightMasterDetailPagePage));
        }
    }
}