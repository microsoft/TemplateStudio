//{[{
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
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
            ViewModel = Ioc.Default.GetService<wts.ItemNameDetailViewModel>();
//}]}
        }
    }
}
