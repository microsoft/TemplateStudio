using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Ioc;
using Param_PageNS.Services;
using Param_PageNS.MapPage;

namespace Param_PageNS
{
    public partial class ViewModelLocator
    {
        public MapPageViewModel MapPageViewModel => ServiceLocator.Current.GetInstance<MapPageViewModel>();

        public void RegisterMapPage(NavigationService navigationService)
        {
            SimpleIoc.Default.Register<MapPageViewModel>();
            navigationService.Configure(typeof(MapPageViewModel).FullName, typeof(MapPagePage));
        }
    }
}