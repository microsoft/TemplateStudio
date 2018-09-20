//{[{
using Param_RootNamespace.ViewModels;
//}]}
namespace Param_RootNamespace.Services
{
    internal class ActivationService
    {
        private readonly Type _defaultNavItem;
//{[{

        private static NavigationServiceEx NavigationService => ViewModelLocator.Current.NavigationService;
//}]}
    }
}
