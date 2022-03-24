//{[{
using Param_RootNamespace.ViewModels;
//}]}

namespace Param_RootNamespace.Views
{
    public sealed partial class wts.ItemNameDetailPage : Page
    {
//{[{
        public wts.ItemNameDetailViewModel ViewModel { get; }
//}]}

        public wts.ItemNameDetailPage()
        {
//{[{
            ViewModel = App.GetService<wts.ItemNameDetailViewModel>();
//}]}
        }
    }
}
