//{[{
using Param_RootNamespace.Helpers;
using Param_RootNamespace.Views;
//}]}

namespace Param_RootNamespace.ViewModels
{
    public class wts.ItemNameViewModel : Observable
    {
        private void OnItemClick(SampleOrder clickedItem)
        {
            if (clickedItem != null)
            {
//^^
//{[{
                NavigationService.Navigate<wts.ItemNameDetailPage>(clickedItem.OrderID);
//}]}
            }
        }
    }
}