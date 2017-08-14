namespace Param_RootNamespace.Services
{
    internal class ActivationService
    {
        private readonly Type _defaultNavItem;
//{[{

        private NavigationServiceEx NavigationService
        {
            get
            {
                return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<NavigationServiceEx>();
            }
        }
//}]}
    }
}
