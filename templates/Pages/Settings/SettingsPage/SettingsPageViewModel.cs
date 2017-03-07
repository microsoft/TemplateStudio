using System;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.Storage;
using RootNamespace.Services;

namespace ItemNamespace.SettingsPage
{
    public class SettingsPageViewModel : System.ComponentModel.INotifyPropertyChanged
    {
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

        public ICommand SwitchThemeCommand { get; private set; }

        public SettingsPageViewModel()
        {
            IsLightThemeEnabled = ThemeSelectorService.IsLightThemeEnabled;
            SwitchThemeCommand = new RelayCommand(SwitchTheme);
            AppDescription = GetAppDescription();
        }

        private void SwitchTheme()
        {
            ThemeSelectorService.SwitchTheme();
        }

        private string GetAppDescription()
        {
            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;
            return $"{package.DisplayName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }
    }
}