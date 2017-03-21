using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using uct.ItemName.PivotPage;
using uct.ItemName.Services;

namespace uct.ItemName
{
    public partial class ViewModelLocator
    {
        public PivotPageViewModel PivotPageViewModel => ServiceLocator.Current.GetInstance<PivotPageViewModel>();

        public void RegisterPivotPage(NavigationService navigationService)
        {
            SimpleIoc.Default.Register<PivotPageViewModel>();
            navigationService.Configure(typeof(PivotPageViewModel).FullName, typeof(PivotPage.PivotPage));
        }
    }
}