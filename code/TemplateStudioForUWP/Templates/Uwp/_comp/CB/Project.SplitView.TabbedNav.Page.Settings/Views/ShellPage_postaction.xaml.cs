//{**
//This code block adds the logic to handle SettingsItem in NavigationView control from ShellPage code behind.
//**}
namespace Param_RootNamespace.Views
{
    public sealed partial class ShellPage : Page, INotifyPropertyChanged
    {
        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            IsBackEnabled = NavigationService.CanGoBack;
            //{[{
            if (e.SourcePageType == typeof(wts.ItemNamePage))
            {
                Selected = navigationView.SettingsItem as WinUI.NavigationViewItem;
                return;
            }

            //}]}
        }

        private void OnItemInvoked(WinUI.NavigationView sender, WinUI.NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
//{--{
                // Navigate to the settings page - implement as appropriate if needed
//}--}
                //{[{
                NavigationService.Navigate(typeof(wts.ItemNamePage), null, args.RecommendedNavigationTransitionInfo);
                //}]}
            }
        }
    }
}
