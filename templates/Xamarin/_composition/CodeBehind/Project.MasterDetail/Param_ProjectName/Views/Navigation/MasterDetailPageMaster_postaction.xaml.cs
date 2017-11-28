//^^
//{[{
using System.Collections.ObjectModel;
using Param_RootNamespace.Models;
//}]}

namespace Param_RootNamespace.Views.Navigation
{
    public partial class MasterDetailPageMaster : ContentPage
    {
        public MasterDetailPageMaster()
        {
            InitializeComponent();
            //^^
            //{[{
            PrimaryMenuItemsListView.ItemsSource = PrimaryMenuItems;
            //}]}
        }        
        //^^
        //{[{
            
        public ObservableCollection<MasterDetailPageMenuItem> PrimaryMenuItems { get; private set; } = new ObservableCollection<MasterDetailPageMenuItem>
        {
        };
        //}]}
    }
}