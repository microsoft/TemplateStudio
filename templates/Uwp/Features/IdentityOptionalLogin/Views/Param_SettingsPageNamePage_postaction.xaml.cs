namespace Param_RootNamespace.Views
{
    public sealed partial class Param_SettingsPageNamePage : Page
    {
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
        }
//^^
//{[{

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            ViewModel.UnregisterEvents();
        }
//}]}
    }
}
