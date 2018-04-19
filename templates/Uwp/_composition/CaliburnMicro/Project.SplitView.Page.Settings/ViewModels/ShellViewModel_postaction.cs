//{**
//This code block adds the logic to handle SettingsItem in NavigationView control from ViewModel.
//**}
using Param_ItemNamespace.Views;
namespace Param_ItemNamespace.ViewModels
{
    public class ShellViewModel : Screen
    {
        private void OnItemInvoked(NavigationViewItemInvokedEventArgs args)
        {
            //{[{
            if (args.IsSettingsInvoked)
            {
                _navigationService.Navigate(typeof(wts.ItemNamePage));
                return;
            }

            //}]}
        }

        private void NavigationService_Navigated(object sender, NavigationEventArgs e)
        {
            //{[{
            if (e.SourcePageType == typeof(wts.ItemNamePage))
            {
                Selected = _navigationView.SettingsItem as NavigationViewItem;
                return;
            }

            //}]}
        }
    }
}
