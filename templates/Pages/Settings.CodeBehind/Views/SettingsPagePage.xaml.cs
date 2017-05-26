using Windows.UI.Xaml.Controls;
using Param_ItemNamespace.Services;
using Windows.ApplicationModel;

namespace Param_ItemNamespace.Views
{
    public sealed partial class SettingsPagePage : Page, System.ComponentModel.INotifyPropertyChanged
    {
        // TODO UWPTemplates: Add other settings as necessary. For help see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/settings.md
        // TODO UWPTemplates: Setup your privacy web in your Resource File, currently set to https://YourPrivacyUrlGoesHere

        private int _selectedTheme;
        public int SelectedTheme
        {
            get { return _selectedTheme; }
            set { Set(ref _selectedTheme, value); }
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
            SelectedTheme = ThemeSelectorService.GetTheme();
        }

        private void Initialize()
        {
            AppDescription = GetAppDescription();
        }

        private string GetAppDescription()
        {
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{package.DisplayName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        private async void Theme_Changed(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                await ThemeSelectorService.SetThemeAsync(comboBox.SelectedIndex);
            }
        }
    }
}
