class DefaultLaunchActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
{
    //{[{
    private readonly string _navElement;

    private NavigationServiceExt NavigationService
    {
        get
        {
            return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<NavigationServiceExt>();
        }
    }

    public DefaultLaunchActivationHandler(Type navElement)
    {
        _navElement = navElement.FullName;
    }

    //}]}
}