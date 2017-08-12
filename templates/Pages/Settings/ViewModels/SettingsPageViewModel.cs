using System;
using System.Windows.Input;
using Param_RootNamespace.Services;
using Windows.ApplicationModel;
using Windows.UI.Xaml;

namespace Param_ItemNamespace.ViewModels
{
    public class SettingsPageViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        // TODO WTS: Add other settings as necessary. For help see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/settings.md
        private ElementTheme _elementTheme = ThemeSelectorService.Theme;

        public ElementTheme ElementTheme
        {
            get { return _elementTheme; }

            set { Set(ref _elementTheme, value); }
        }

        private string _versionDescription;

        public string VersionDescription
        {
            get { return _versionDescription; }

            set { Set(ref _versionDescription, value); }
        }

        public ICommand SwitchThemeCommand { get; private set; }

        public SettingsPageViewModel()
        {
            SwitchThemeCommand = new RelayCommand<ElementTheme>(
                async (param) =>
                {
                    await ThemeSelectorService.SetThemeAsync(param);
                });
        }

        public void Initialize()
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
    }
}
