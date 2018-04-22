//{**
//This code block adds the logic to handle SettingsItem in NavigationView control from ShellPage code behind.
//**}
namespace Param_ItemNamespace.Views
{
    public sealed partial class ShellPage : Page, INotifyPropertyChanged
    {
        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            //{[{
            if (e.SourcePageType == typeof(wts.ItemNamePage))
            {
                Selected = navigationView.SettingsItem as NavigationViewItem;
                return;
            }

            //}]}
        }

        private void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            //{[{
            if (args.IsSettingsInvoked)
            {
                NavigationService.Navigate(typeof(wts.ItemNamePage));
                return;
            }

            //}]}
        }
    }
}
