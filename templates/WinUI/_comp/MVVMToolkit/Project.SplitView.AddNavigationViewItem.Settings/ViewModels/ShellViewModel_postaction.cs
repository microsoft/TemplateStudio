namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : ObservableRecipient
    {

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            IsBackEnabled = _navigationService.CanGoBack;
//{[{
            if (e.SourcePageType == typeof(wts.ItemNamePage))
            {
                Selected = _navigationView.SettingsItem as NavigationViewItem;
                return;
            }
//}]}
        }

        private void OnItemInvoked(NavigationViewItemInvokedEventArgs args)
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
