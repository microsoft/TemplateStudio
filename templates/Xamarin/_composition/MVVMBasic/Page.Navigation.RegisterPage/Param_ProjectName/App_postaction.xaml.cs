//{[{
using Param_RootNamespace.Views;
using Param_RootNamespace.ViewModels;
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
            navigationService.Register<wts.ItemNameViewModel>(typeof(wts.ItemNamePage));
            //}]}
        }
    }
}
