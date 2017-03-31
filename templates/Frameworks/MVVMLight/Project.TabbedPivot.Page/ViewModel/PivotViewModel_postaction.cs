using ItemNamespace.Models;
using ItemNamespace.View;
using ItemNamespace.Helper;
namespace ItemNamespace.ViewModel
{
    public class PivotViewModel : ViewModelBase
    {
        public PivotViewModel()
        {
            //^^
            Items.Add(new PivotTabbedItem("PivotView_uct.ItemName".GetLocalized(), new uct.ItemNameView()));
        }
    }
}