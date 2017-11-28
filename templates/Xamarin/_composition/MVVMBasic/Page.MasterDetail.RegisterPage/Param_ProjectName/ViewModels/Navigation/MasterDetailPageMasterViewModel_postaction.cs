namespace Param_RootNamespace.ViewModels.Navigation
{
    public class MasterDetailPageMasterViewModel : Observable
    {
        public ObservableCollection<MasterDetailPageMenuItem> PrimaryMenuItems { get; private set; } = new ObservableCollection<MasterDetailPageMenuItem>
        {
            //^^
            //{[{
            new MasterDetailPageMenuItem { Title = "wts.ItemName", TargetType = typeof(wts.ItemNamePage), IconSource = "blank.png"},
            //}]}
        };
    }
}
