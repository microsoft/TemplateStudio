using System;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.Storage;

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
            ApplicationData.Current.LocalSettings.Values["RequestedTheme"] = (IsLightThemeEnabled) ? "Light" : "Dark";
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
            string themeName;
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("RequestedTheme"))
            {
                themeName = ApplicationData.Current.LocalSettings.Values["RequestedTheme"].ToString();
            }
            else
            {
                themeName = Application.Current.RequestedTheme.ToString();
            }
            IsLightThemeEnabled = themeName == "Light";
        } 


        //TODO: MOVE THIS SOMEWHERE
        public static void InitAppTheme()
        {
            //Set application theme saved on application settings
            if (Windows.Storage.ApplicationData.Current.LocalSettings.Values.ContainsKey("RequestedTheme"))
            {
                string themeName = Windows.Storage.ApplicationData.Current.LocalSettings.Values["RequestedTheme"].ToString();
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
}