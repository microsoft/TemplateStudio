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
            InitializeTheme();
            SwitchThemeCommand = new RelayCommand(SwitchTheme);
            AppDescription = GetAppDescription();
        }

        private void SwitchTheme()
        {
            ElementTheme theme = IsLightThemeEnabled ? ElementTheme.Light : ElementTheme.Dark;
            (Window.Current.Content as FrameworkElement).RequestedTheme = theme;
            SettingsStorageService.Save<string>("RequestedTheme", (IsLightThemeEnabled) ? "Light" : "Dark", Windows.Storage.ApplicationData.Current.LocalSettings);
        }

        private string GetAppDescription()
        {
            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;
            return $"{package.DisplayName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        private void InitializeTheme()
        {
            var themeName = SettingsStorageService.Read<string>("RequestedTheme", ApplicationData.Current.LocalSettings);

            if (String.IsNullOrEmpty(themeName))
            {
                themeName = Application.Current.RequestedTheme.ToString();
            }

            IsLightThemeEnabled = themeName == "Light";
        } 


        //TODO: MOVE THIS SOMEWHERE
        public static void InitAppTheme()
        {
            //Set application theme saved on application settings
            var themeName = SettingsStorageService.Read<string>("RequestedTheme", ApplicationData.Current.LocalSettings);
            if (themeName == null)
            {
                return;
            }
            if (themeName.Equals("Light"))
            {
                (Window.Current.Content as FrameworkElement).RequestedTheme = ElementTheme.Light;
            }
            else if (themeName.Equals("Dark"))
            {
                (Window.Current.Content as FrameworkElement).RequestedTheme = ElementTheme.Dark;
            }
        }
    }
}