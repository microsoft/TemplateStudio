namespace Param_RootNamespace.Views
{
    public sealed partial class SettingsPage : Page
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
