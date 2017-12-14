using Windows.UI.Xaml.Controls;

using WTSPrismNavigationBase.ViewModels;

namespace WTSPrismNavigationBase.Views
{
    public sealed partial class SettingsPage : Page
    {
        private SettingsPageViewModel ViewModel
        {
            get { return DataContext as SettingsPageViewModel; }
        }

        // TODO WTS: Setup your privacy web in your Resource File, currently set to https://YourPrivacyUrlGoesHere

        public SettingsPage()
        {
            InitializeComponent();
        }
    }
}
