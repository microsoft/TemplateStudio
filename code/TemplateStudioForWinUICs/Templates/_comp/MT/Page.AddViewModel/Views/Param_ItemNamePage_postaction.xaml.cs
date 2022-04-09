//{[{
using Param_RootNamespace.ViewModels;
//}]}

namespace Param_RootNamespace.Views
{
    public sealed partial class Param_ItemNamePage : Page
    {
//{[{
        public Param_ItemNameViewModel ViewModel { get; }
//}]}

        public Param_ItemNamePage()
        {
//{[{
            ViewModel = App.GetService<Param_ItemNameViewModel>();
//}]}
        }
    }
}
