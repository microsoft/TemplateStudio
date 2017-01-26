using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Page_NS.Services;
using Page_NS.MVVMLightSettingsPage;

namespace Page_NS
{
    public partial class ViewModelLocator
    {
        public MVVMLightSettingsPageViewModel MVVMLightSettingsPageViewModel => ServiceLocator.Current.GetInstance<MVVMLightSettingsPageViewModel>();

        public void RegisterMVVMLightSettingsPage(NavigationService navigationService)
        {
            SimpleIoc.Default.Register<MVVMLightSettingsPageViewModel>();
            navigationService.Configure(typeof(MVVMLightSettingsPageViewModel).FullName, typeof(MVVMLightSettingsPagePage));
        }
    }
}