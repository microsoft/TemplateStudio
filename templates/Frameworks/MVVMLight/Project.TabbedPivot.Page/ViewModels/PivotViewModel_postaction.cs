using ItemNamespace.Models;
using ItemNamespace.Views;
using ItemNamespace.Helper;
namespace ItemNamespace.ViewModels
{
    public class PivotViewModel : ViewModelBase
    {
        public PivotViewModel()
        {
            //^^
            Items.Add(new PivotTabbedItem("PivotPage_uct.ItemName".GetLocalized(), new uct.ItemNamePage()));
        }
    }
}