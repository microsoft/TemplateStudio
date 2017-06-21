using Windows.UI.Xaml.Controls;
using Param_ItemNamespace.Services;
using Windows.ApplicationModel;

namespace Param_ItemNamespace.Views
{
    public sealed partial class SettingsPagePage : Page, System.ComponentModel.INotifyPropertyChanged
    {
        //// TODO UWPTemplates: Add other settings as necessary. For help see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/settings.md
        //// TODO UWPTemplates: Setup your privacy web in your Resource File, currently set to https://YourPrivacyUrlGoesHere

        private bool _isLightThemeEnabled;

        public bool IsLightThemeEnabled
        {
            get { return _isLightThemeEnabled; }
            set { Set(ref _isLightThemeEnabled, value); }
        }

        private string _appDescription;

        public string AppDescription
        {
            get { return _appDescription; }
            set { Set(ref _appDescription, value); }
        }

        public SettingsPagePage()
        {
            InitializeComponent();
        }

        private void Initialize()
        {
            IsLightThemeEnabled = ThemeSelectorService.IsLightThemeEnabled;
            AppDescription = GetAppDescription();
        }

        private string GetAppDescription()
        {
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{package.DisplayName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        private async void ThemeToggle_Toggled(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // Only switch theme if value has changed (not on initialization)
            var toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn != ThemeSelectorService.IsLightThemeEnabled)
                {
                    await ThemeSelectorService.SwitchThemeAsync();
                }
            }
        }
    }
}
