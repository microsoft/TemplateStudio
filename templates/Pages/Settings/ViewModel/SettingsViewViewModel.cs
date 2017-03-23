using System;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.Storage;
using RootNamespace.Services;

namespace ItemNamespace.ViewModel
{
    public class SettingsViewViewModel : System.ComponentModel.INotifyPropertyChanged
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

        public SettingsViewViewModel()
        {
            IsLightThemeEnabled = ThemeSelectorService.IsLightThemeEnabled;
            SwitchThemeCommand = new RelayCommand(async () => { await ThemeSelectorService.SwitchThemeAsync(); });
        }

        public void Initialize()
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
    }
}