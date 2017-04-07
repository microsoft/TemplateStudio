using ItemNamespace.Services;
namespace ItemNamespace.ViewModels
{
    public class uct.ItemNameViewModel : Observable
    {
        private void OnItemClick(ItemClickEventArgs args)
        {
            SampleModel item = args?.ClickedItem as SampleModel;
            if (item != null)
            {
                if (_currentState.Name == NarrowStateName)
                {
                    NavigationService.Navigate<Views.uct.ItemNameDetailPage>(item);
                }
                else
                {
                    Selected = item;
                }
            }
        }
    }
}