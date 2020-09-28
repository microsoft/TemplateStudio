//{[{
using Param_RootNamespace.Constants;
//}]}
namespace Param_RootNamespace.ViewModels
{
    public class wts.ItemNameViewModel : BindableBase, INavigationAware
    {
        private readonly ISampleDataService _sampleDataService;
//{[{
        private readonly IRegionManager _regionManager;
        private IRegionNavigationService _navigationService;
//}]}

        public wts.ItemNameViewModel(/*{[{*/IRegionManager regionManager/*}]}*/)
        {
//^^
//{[{
            _regionManager = regionManager;
            _navigationService = regionManager.Regions[Regions.Main].NavigationService;
//}]}
        }

        private void NavigateToDetail(SampleOrder order)
        {
//^^
//{[{
            _navigationService.RequestNavigate(PageKeys.wts.ItemNameDetail, new NavigationParameters() { { "DetailID", order.OrderID } });
//}]}
        }
    }
}