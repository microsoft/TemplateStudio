//{**
// This code block adds the code to launch the Feedback Hub from the settings page
//**}
namespace Param_RootNamespace.ViewModels
{
    public class Param_SettingsPageNameViewModel : Screen
    {

//{[{
        public Visibility FeedbackLinkVisibility => Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.IsSupported() ? Visibility.Visible : Visibility.Collapsed;

        public async void LaunchFeedbackHub()
        {
            // This launcher is part of the Store Services SDK https://docs.microsoft.com/windows/uwp/monetize/microsoft-store-services-sdk
            var launcher = Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.GetDefault();
            await launcher.LaunchAsync();
        }

//}]}
    }
}
