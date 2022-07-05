//{[{
using Param_RootNamespace.Constants;
//}]}
namespace Param_RootNamespace.ViewModels;

public class ts.ItemNameViewModel : BindableBase, INavigationAware
{
    private readonly ISampleDataService _sampleDataService;
//{[{
    private readonly IRegionManager _regionManager;
    private IRegionNavigationService _navigationService;
//}]}

    public ts.ItemNameViewModel(/*{[{*/IRegionManager regionManager/*}]}*/)
    {
//^^
//{[{
        _regionManager = regionManager;
        if (regionManager.Regions.ContainsRegionWithName(Regions.Main))
        {
            _navigationService = regionManager.Regions[Regions.Main].NavigationService;
        }
//}]}
    }

    private void NavigateToDetail(SampleOrder order)
    {
//^^
//{[{
        _navigationService.RequestNavigate(PageKeys.ts.ItemNameDetail, new NavigationParameters() { { "DetailID", order.OrderID } });
//}]}
    }
}
