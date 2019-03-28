//{[{
using Param_RootNamespace.ViewModels;
//}]}

namespace Param_RootNamespace.Services
{
    internal class ActivationService
    {

        private IdentityService IdentityService => Singleton<IdentityService>.Instance;
//{[{

        public static NavigationServiceEx NavigationService => ViewModelLocator.Current.NavigationService;
//}]}
//^^
//{[{

        public void SetShell(Lazy<UIElement> shell)
        {
            _shell = shell;
        }
//}]}
    }
}
