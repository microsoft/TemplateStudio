using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Page_NS.Services;
using Page_NS.MVVMLightWebViewPage;

namespace Page_NS
{
    public partial class ViewModelLocator
    {
        public MVVMLightWebViewPageViewModel MVVMLightWebViewPageViewModel => ServiceLocator.Current.GetInstance<MVVMLightWebViewPageViewModel>();

        public void RegisterMVVMLightWebViewPage(NavigationService navigationService)
        {
            SimpleIoc.Default.Register<MVVMLightWebViewPageViewModel>();
            navigationService.Configure(typeof(MVVMLightWebViewPageViewModel).FullName, typeof(MVVMLightWebViewPagePage));
        }
    }
}