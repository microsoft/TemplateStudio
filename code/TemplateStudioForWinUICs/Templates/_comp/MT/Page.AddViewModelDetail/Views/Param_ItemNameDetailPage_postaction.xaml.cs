//{[{
using Param_RootNamespace.ViewModels;
//}]}

namespace Param_RootNamespace.Views;

public sealed partial class Param_ItemNameDetailPage : Page
{
//{[{
    public Param_ItemNameDetailViewModel ViewModel
    {
        get;
    }
//}]}

    public Param_ItemNameDetailPage()
    {
//{[{
        ViewModel = App.GetService<Param_ItemNameDetailViewModel>()!;
//}]}
    }
}
