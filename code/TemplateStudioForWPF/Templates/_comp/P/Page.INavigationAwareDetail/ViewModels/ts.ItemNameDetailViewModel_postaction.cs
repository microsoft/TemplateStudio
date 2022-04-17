//{[{
using Prism.Regions;
//}]}
namespace Param_RootNamespace.ViewModels
{
    public class ts.ItemNameDetailViewModel : BindableBase, INavigationAware
    {
//^^
//{[{

        public bool IsNavigationTarget(NavigationContext navigationContext)
            => true;
//}]}
    }
}
