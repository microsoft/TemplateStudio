namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : ObservableRecipient
    {

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            IsBackEnabled = NavigationService.CanGoBack;
//{[{
            if (e.SourcePageType == typeof(Param_ItemNamePage))
            {
                Selected = NavigationViewService.SettingsItem;
                return;
            }
//}]}
        }
    }
}
