using ItemNamespace.View;
using ItemNamespace.Helper;
namespace ItemNamespace.ViewModel
{
    public class PivotViewModel : Observable
    {
        public PivotViewModel()
        {
            //^^
            Items.Add(new PivotTabbedItem("PivotView_uct.ItemName".GetLocalized(), new uct.ItemNameView()));
        }
    }
}