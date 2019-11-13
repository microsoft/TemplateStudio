using System;
using System.Windows.Input;
using Param_RootNamespace.Helpers;
using Param_RootNamespace.Services;
using Windows.ApplicationModel;
using Windows.UI.Xaml;

namespace Param_RootNamespace.ViewModels
{
    // TODO WTS: Add other settings as necessary. For help see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/UWP/pages/settings.md
    public class SettingsPageViewModel : System.ComponentModel.INotifyPropertyChanged
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

        public async void SwitchTheme(ElementTheme theme)
        {
            await ThemeSelectorService.SetThemeAsync(theme);
        }

        public SettingsPageViewModel()
        {
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            VersionDescription = GetVersionDescription();
        }

        private string GetVersionDescription()
        {
            var appName = "AppDisplayName".GetLocalized();
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{appName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }
    }
}
