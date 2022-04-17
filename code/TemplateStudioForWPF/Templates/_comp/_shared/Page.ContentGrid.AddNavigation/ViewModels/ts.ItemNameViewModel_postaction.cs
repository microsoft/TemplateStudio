//{[{
using Param_RootNamespace.Contracts.Services;
//}]}
namespace Param_RootNamespace.ViewModels
{
    public class ts.ItemNameViewModel : System.ComponentModel.INotifyPropertyChanged, INavigationAware
    {
//{[{
        private readonly INavigationService _navigationService;
//}]}
        public ts.ItemNameViewModel(/*{[{*/INavigationService navigationService/*}]}*/)
        {
//^^
//{[{
            _navigationService = navigationService;
//}]}
        }

        private void NavigateToDetail(SampleOrder order)
        {
//^^
//{[{
            _navigationService.NavigateTo(typeof(ts.ItemNameDetailViewModel).FullName, order.OrderID);
//}]}
        }
    }
}
