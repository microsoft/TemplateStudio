namespace Param_RootNamespace.Services
{
    internal class ActivationService
    {
        private readonly Type _defaultNavItem;
//{[{

        private static ViewModels.ViewModelLocator Locator => Application.Current.Resources["Locator"] as ViewModels.ViewModelLocator;

        private static NavigationServiceEx NavigationService => Locator.NavigationService;
//}]}
    }
}
