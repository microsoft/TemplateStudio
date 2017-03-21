namespace ItemNamespace.PivotPage
{
    public class PivotPageViewModel : ViewModelBase
    {
        public PivotPageViewModel()
        {
            //^^
            Items.Add(new PivotTabbedItem("PivotPage_uct.ItemName".GetLocalized(), new uct.ItemName.uct.ItemNamePage()));
        }
    }
}