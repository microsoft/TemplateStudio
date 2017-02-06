using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Ioc;
using Param_PageNS.Services;
using Param_PageNS.MasterDetailPage;

namespace Param_PageNS
{
    public partial class ViewModelLocator
    {
        public MasterDetailPageViewModel MasterDetailPageViewModel => ServiceLocator.Current.GetInstance<MasterDetailPageViewModel>();

        public void RegisterMasterDetailPage(NavigationService navigationService)
        {
            SimpleIoc.Default.Register<MasterDetailPageViewModel>();
            navigationService.Configure(typeof(MasterDetailPageViewModel).FullName, typeof(MasterDetailPagePage));
        }
    }
}