//{[{
using Param_RootNamespace.Contracts.Services;
//}]}
namespace Param_RootNamespace.ViewModels
{
    public class wts.ItemNameViewModel : System.ComponentModel.INotifyPropertyChanged, INavigationAware
    {
//{[{
        private readonly INavigationService _navigationService;
//}]}
        public wts.ItemNameViewModel(/*{[{*/INavigationService navigationService/*}]}*/)
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
            _navigationService.NavigateTo(typeof(wts.ItemNameDetailViewModel).FullName, order.OrderID);
//}]}
        }
    }
}