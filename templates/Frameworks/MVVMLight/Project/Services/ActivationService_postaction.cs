class ActivationService
{
    private readonly Type _defaultNavItem;
    //{[{

    private NavigationServiceExt NavigationService
    {
        get
        {
            return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<NavigationServiceExt>();
        }
    }
    
    //}]}
}