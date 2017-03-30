using ItemNamespace.Views;
using ItemNamespace.Helper;
namespace ItemNamespace.ViewModels
{
    public class PivotViewModel : Observable
    {
        public PivotViewModel()
        {
            //^^
            Items.Add(new PivotTabbedItem("PivotPage_uct.ItemName".GetLocalized(), new uct.ItemNamePage()));
        }
    }
}