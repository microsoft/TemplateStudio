using Param_ItemNamespace.Services;
//{[{
using Param_ItemNamespace.Models;
//}]}
namespace Param_ItemNamespace.ViewModels
{
    public class wts.ItemNameViewModel : Observable
    {
        private void OnItemClick(ItemClickEventArgs args)
        {
            //{[{
            SampleOrder item = args?.ClickedItem as SampleOrder;
            if (item != null)
            {
                if (_currentState.Name == NarrowStateName)
                {
                    NavigationService.Navigate<Views.wts.ItemNameDetailPage>(item);
                }
                else
                {
                    Selected = item;
                }
            }
            //}]}
        }
    }
}
