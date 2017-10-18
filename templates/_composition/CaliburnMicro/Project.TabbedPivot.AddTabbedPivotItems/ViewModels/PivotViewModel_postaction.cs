namespace Param_ItemNamespace.ViewModels
{
    public class PivotViewModel : Conductor<Screen>.Collection.OneActive
    {
        protected override void OnInitialize()
        {
            //^^
            //{[{
            Items.Add(new wts.ItemNameViewModel { DisplayName = "PivotItem_wts.ItemName/Header".GetLocalized() });
            //}]}
        }
    }
}
