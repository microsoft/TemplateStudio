//{**
// This code block adds the code to launch the Feedback Hub from the settings page
//**}
namespace Param_RootNamespace.Views
{
    public sealed partial class Param_SettingsPageNamePage : Page, INotifyPropertyChanged
    {
        private async Task InitializeAsync()
        {
            //^^
            //{[{

            if (Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.IsSupported())
            {
                FeedbackLink.Visibility = Visibility.Visible;
            }
            //}]}
        }

        //^^
        //{[{
        private async void FeedbackLink_Click(object sender, RoutedEventArgs e)
        {
            // This launcher is part of the Store Services SDK https://docs.microsoft.com/windows/uwp/monetize/microsoft-store-services-sdk
            var launcher = Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.GetDefault();
            await launcher.LaunchAsync();
        }
        //}]}
    }
}
