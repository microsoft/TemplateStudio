//{**
//This code block adds the logic to handle SettingsItem in NavigationView control from ViewModel.
//**}
namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : Screen
    {
        private void OnItemInvoked(WinUI.NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
//{--{
                // Navigate to the settings page - implement as appropriate if needed
//}--}
                //{[{
                _navigationService.Navigate(typeof(wts.ItemNamePage));
                //}]}
            }
        }

        private void NavigationService_Navigated(object sender, NavigationEventArgs e)
        {
            IsBackEnabled = _navigationService.CanGoBack;
            //{[{
            if (e.SourcePageType == typeof(wts.ItemNamePage))
            {
                Selected = _navigationView.SettingsItem as WinUI.NavigationViewItem;
                return;
            }

            //}]}
        }
    }
}
