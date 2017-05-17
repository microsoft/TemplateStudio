using Windows.UI.Xaml.Controls;
using Param_ItemNamespace.Services;
using Windows.ApplicationModel;

namespace Param_ItemNamespace.Views
{
    public sealed partial class SettingsPagePage : Page, System.ComponentModel.INotifyPropertyChanged
    {
        // TODO UWPTemplates: Add other settings as necessary. For help see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/settings.md
        // TODO UWPTemplates: Setup your privacy web in your Resource File, currently set to https://YourPrivacyUrlGoesHere

        private bool _isLightThemeEnabled;
        public bool IsLightThemeEnabled
        {
            get { return _isLightThemeEnabled; }
            set { Set(ref _isLightThemeEnabled, value); }
        }

        private bool _isDarkThemeEnabled;
        public bool IsDarkThemeEnabled
        {
            get { return _isDarkThemeEnabled; }
            set { Set(ref _isDarkThemeEnabled, value); }
        }

        private bool _isDefaultThemeEnabled;
        public bool IsDefaultThemeEnabled
        {
            get { return _isDefaultThemeEnabled; }
            set { Set(ref _isDefaultThemeEnabled, value); }
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
            IsDarkThemeEnabled = ThemeSelectorService.IsDarkThemeEnabled;
            IsDefaultThemeEnabled = ThemeSelectorService.IsDefaultThemeEnabled;
            AppDescription = GetAppDescription();
        }

        private string GetAppDescription()
        {
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{package.DisplayName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        private async void Theme_Checked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var radioButton = sender as RadioButton;
            if (radioButton != null)
            {
                string themeName = radioButton.Tag.ToString();
                await ThemeSelectorService.SetThemeAsync(themeName);
            }
        }
    }
}
