namespace Param_RootNamespace.Services
{
    public class NavigationViewService : INavigationViewService
    {

        private void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
//{--{
                // Navigate to the settings page.
//}--}
//{[{
                _navigationService.NavigateTo(typeof(Param_ItemNameViewModel).FullName);
//}]}
            }
        }
    }
}
