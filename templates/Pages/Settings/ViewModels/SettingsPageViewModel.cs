using System;
using System.Windows.Input;
using Windows.ApplicationModel;
using Param_RootNamespace.Services;

namespace Param_ItemNamespace.ViewModels
{
    public class SettingsPageViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        // TODO UWPTemplates: Add other settings as necessary. For help see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/settings.md
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

        public ICommand SelectThemeCommand { get; private set; }

        public SettingsPageViewModel()
        {
            SelectThemeCommand = new RelayCommand<string>(async (string themeName) => { await ThemeSelectorService.SetThemeAsync(themeName); });
        }

        public void Initialize()
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
    }
}
