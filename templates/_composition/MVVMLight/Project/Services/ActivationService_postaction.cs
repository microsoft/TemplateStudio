namespace Param_RootNamespace.Services
{
    internal class ActivationService
    {
        private readonly Type _defaultNavItem;
//{[{

        private ViewModels.ViewModelLocator Locator => Application.Current.Resources["Locator"] as ViewModels.ViewModelLocator;

        private NavigationServiceEx NavigationService => Locator.NavigationService;
//}]}
    }
}
