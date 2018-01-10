using System;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using WtsXamarinUWP.UWP.ViewModels;

namespace WtsXamarinUWP.UWP.Views
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsViewModel ViewModel { get; } = new SettingsViewModel();

        //// TODO WTS: Change the URL for your privacy policy in the Resource File, currently set to https://YourPrivacyUrlGoesHere

        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.Initialize();
        }
    }
}
