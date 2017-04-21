using Param_ItemNamespace.Services;
namespace Param_ItemNamespace.ViewModels
{
    public class wts.ItemNameViewModel : Observable
    {
        private void OnItemClick(ItemClickEventArgs args)
        {
            SampleModel item = args?.ClickedItem as SampleModel;
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
        }
    }
}