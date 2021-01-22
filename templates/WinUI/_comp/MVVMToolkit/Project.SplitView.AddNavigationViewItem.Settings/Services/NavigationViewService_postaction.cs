namespace Param_RootNamespace.Services
{
    public class NavigationViewService : INavigationViewService
    {

        private void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
//{--{
                // Navigate to the settings page - implement as appropriate if needed
//}--}
//{[{
                _navigationService.NavigateTo(typeof(wts.ItemNameViewModel).FullName);
//}]}
                return;
            }
        }
    }
}
