//{**
// This code block adds the code to launch the Feedback Hub from the settings page
//**}
namespace Param_ItemNamespace.Views
{
    public sealed partial class Param_SettingsPageNamePage : Page, INotifyPropertyChanged
    {
        private void Initialize()
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
            // This launcher is part of the Store Services SDK https://docs.microsoft.com/en-us/windows/uwp/monetize/microsoft-store-services-sdk
            var launcher = Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.GetDefault();
            await launcher.LaunchAsync();
        }
        //}]}
    }
}
