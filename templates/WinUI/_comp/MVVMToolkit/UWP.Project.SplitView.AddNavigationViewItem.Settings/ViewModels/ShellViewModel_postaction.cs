namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : ObservableRecipient
    {

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            IsBackEnabled = NavigationService.CanGoBack;
//{[{
            if (e.SourcePageType == typeof(wts.ItemNamePage))
            {
                Selected = NavigationViewService.SettingsItem;
                return;
            }
//}]}
        }
    }
}
