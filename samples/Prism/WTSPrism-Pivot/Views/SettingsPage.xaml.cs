using Windows.UI.Xaml.Controls;
using WTSPrism.ViewModels;

namespace WTSPrism.Views
{
    public sealed partial class SettingsPage : Page
    {
        private SettingsPageViewModel ViewModel => DataContext as SettingsPageViewModel;

        // TODO WTS: Setup your privacy web in your Resource File, currently set to https://YourPrivacyUrlGoesHere

        public SettingsPage()
        {
            InitializeComponent();
        }
    }
}
