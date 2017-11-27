//^^
//{[{
using System.Collections.ObjectModel;
using Param_RootNamespace.Models;
//}]}

namespace Param_RootNamespace.Views.Navigation
{
    public partial class MasterDetailPageMaster : ContentPage
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