using ItemNamespace.Services;
namespace ItemNamespace.ViewModel
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
                    NavigationService.Navigate<View.uct.ItemNameDetailView>(item);
                }
                else
                {
                    Selected = item;
                }
            }
        }
    }
}