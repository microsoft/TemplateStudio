//^^
//{[{
using Param_RootNamespace.ViewModels.Navigation;
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
            BindingContext = new MasterDetailPageMasterViewModel();
            //}]}
        }
    }
}