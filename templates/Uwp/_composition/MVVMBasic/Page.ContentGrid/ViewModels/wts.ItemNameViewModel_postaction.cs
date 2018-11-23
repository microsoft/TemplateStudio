//{[{
using Param_ItemNamespace.Helpers;
using Param_ItemNamespace.Views;
//}]}

namespace Param_ItemNamespace.ViewModels
{
    public class wts.ItemNameViewModel : Observable
    {
        private void OnItemClick(SampleOrder clickedItem)
        {
            if (clickedItem != null)
            {
//{[{
                NavigationService.Navigate<wts.ItemNameDetailPage>(clickedItem);
//}]}
            }
        }
    }
}