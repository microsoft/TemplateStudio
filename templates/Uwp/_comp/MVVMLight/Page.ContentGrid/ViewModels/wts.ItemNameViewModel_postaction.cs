//{[{
using GalaSoft.MvvmLight.Command;
//}]}

namespace Param_RootNamespace.ViewModels
{
    public class wts.ItemNameViewModel : ViewModelBase
    {
//{[{
        public NavigationServiceEx NavigationService => ViewModelLocator.Current.NavigationService;

//}]}
        private void OnItemClick(SampleOrder clickedItem)
        {
            if (clickedItem != null)
            {
//^^
//{[{
                NavigationService.Navigate(typeof(wts.ItemNameDetailViewModel).FullName, clickedItem.OrderID);
//}]}
            }
        }
    }
}