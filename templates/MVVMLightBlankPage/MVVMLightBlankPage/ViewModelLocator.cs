using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Page_NS.Services;
using Page_NS.MVVMLightBlankPage;

namespace Page_NS
{
    public partial class ViewModelLocator
    {
        public MVVMLightBlankPageViewModel MVVMLightBlankPageViewModel => ServiceLocator.Current.GetInstance<MVVMLightBlankPageViewModel>();

        public void RegisterMVVMLightBlankPage(NavigationService navigationService)
        {
            SimpleIoc.Default.Register<MVVMLightBlankPageViewModel>();
            navigationService.Configure(typeof(MVVMLightBlankPageViewModel).FullName, typeof(MVVMLightBlankPagePage));
        }
    }
}