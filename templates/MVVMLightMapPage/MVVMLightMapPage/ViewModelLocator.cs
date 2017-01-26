using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Page_NS.Services;
using Page_NS.MVVMLightMapPage;

namespace Page_NS
{
    public partial class ViewModelLocator
    {
        public MVVMLightMapPageViewModel MVVMLightMapPageViewModel => ServiceLocator.Current.GetInstance<MVVMLightMapPageViewModel>();

        public void RegisterMVVMLightMapPage(NavigationService navigationService)
        {
            SimpleIoc.Default.Register<MVVMLightMapPageViewModel>();
            navigationService.Configure(typeof(MVVMLightMapPageViewModel).FullName, typeof(MVVMLightMapPagePage));
        }
    }
}