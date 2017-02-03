using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Ioc;
using Param_PageNS.Services;
using Param_PageNS.SettingsPage;

namespace Param_PageNS
{
    public partial class ViewModelLocator
    {
        public SettingsPageViewModel SettingsPageViewModel => ServiceLocator.Current.GetInstance<SettingsPageViewModel>();

        public void RegisterSettingsPage(NavigationService navigationService)
        {
            SimpleIoc.Default.Register<SettingsPageViewModel>();
            navigationService.Configure(typeof(SettingsPageViewModel).FullName, typeof(SettingsPagePage));
        }
    }
}