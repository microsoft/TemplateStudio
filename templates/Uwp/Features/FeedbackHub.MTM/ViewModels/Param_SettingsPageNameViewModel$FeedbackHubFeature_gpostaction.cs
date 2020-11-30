//{**
// This code block adds the code to launch the Feedback Hub from the settings page
//**}
//{[{
using Microsoft.Toolkit.Mvvm.Input;
//}]}

namespace Param_RootNamespace.ViewModels
{
    public class Param_SettingsPageNameViewModel : ObservableObject
    {
//^^
//{[{
        public Visibility FeedbackLinkVisibility => Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.IsSupported() ? Visibility.Visible : Visibility.Collapsed;

        private ICommand _launchFeedbackHubCommand;

        public ICommand LaunchFeedbackHubCommand
        {
            get
            {
                if (_launchFeedbackHubCommand == null)
                {
                    _launchFeedbackHubCommand = new RelayCommand(
                        async () =>
                        {
                            // This launcher is part of the Store Services SDK https://docs.microsoft.com/windows/uwp/monetize/microsoft-store-services-sdk
                            var launcher = Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.GetDefault();
                            await launcher.LaunchAsync();
                        });
                }

                return _launchFeedbackHubCommand;
            }
        }

//}]}
        public Param_SettingsPageNameViewModel()
        {
        }
    }
}
