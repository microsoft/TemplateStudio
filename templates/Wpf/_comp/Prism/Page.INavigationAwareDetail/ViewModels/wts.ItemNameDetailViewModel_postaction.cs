//{[{
using Prism.Regions;
//}]}
namespace Param_RootNamespace.ViewModels
{
    public class wts.ItemNameDetailViewModel : BindableBase, INavigationAware
    {
//^^
//{[{

        public bool IsNavigationTarget(NavigationContext navigationContext)
            => true;
//}]}
    }
}
