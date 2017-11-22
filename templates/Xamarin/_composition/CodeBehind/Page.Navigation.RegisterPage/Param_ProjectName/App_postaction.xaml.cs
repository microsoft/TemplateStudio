//{[{
using Param_RootNamespace.Views;
//}]}
namespace Param_RootNamespace
{
    public partial class App : Application
    {
        private void RegisterNavigationPages()
        {
            var navigationService = NavigationService.Instance;
            //^^
            //{[{
            navigationService.Register("wts.ItemName", typeof(wts.ItemNamePage));
            //}]}
        }
    }
}
