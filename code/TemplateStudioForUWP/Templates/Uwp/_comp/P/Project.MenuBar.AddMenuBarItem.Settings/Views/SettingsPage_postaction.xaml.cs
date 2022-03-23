//{[{
using Windows.UI.Xaml.Navigation;
//}]}
namespace Param_RootNamespace.Views
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
        }
//{[{

        // If this SettingsPage is opened in the right pane of the MenuBar project, we use
        // these methods instead of OnNavigatedTo and OnNavigatingFrom in the ViewModel.
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await ViewModel.InitializeAsync();
        }
//}]}
    }
}
