using System;
using Param_ItemNamespace.Services;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Param_ItemNamespace.Views
{
    // TODO WTS: Add other settings as necessary. For help see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/settings-codebehind.md
    // TODO WTS: Change the URL for your privacy policy in the Resource File, currently set to https://YourPrivacyUrlGoesHere
    public sealed partial class SettingsPagePage : Page, System.ComponentModel.INotifyPropertyChanged
    {
        private ElementTheme _elementTheme = ThemeSelectorService.Theme;

        public ElementTheme ElementTheme
        {
            get { return _elementTheme; }

            set { Param_Setter(ref _elementTheme, value); }
        }

        private string _versionDescription;

        public string VersionDescription
        {
            get { return _versionDescription; }

            set { Param_Setter(ref _versionDescription, value); }
        }

        public SettingsPagePage()
        {
            InitializeComponent();
        }

        private void Initialize()
        {
            VersionDescription = GetVersionDescription();
        }

        private string GetVersionDescription()
        {
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{package.DisplayName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        private async void ThemeChanged_CheckedAsync(object sender, RoutedEventArgs e)
        {
            var param = (sender as RadioButton)?.CommandParameter;

            if (param != null)
            {
                await ThemeSelectorService.SetThemeAsync((ElementTheme)param);
            }
        }
    }
}
