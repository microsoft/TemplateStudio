using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.Storage;
using Page_NS.Core;

namespace Page_NS.BasicSettingsPage
{
    public class BasicSettingsPageViewModel : ViewModelBase
    {
        private bool _isLightThemeEnabled;
        public bool IsLightThemeEnabled
        {
            get => _isLightThemeEnabled;
            set => Set(ref _isLightThemeEnabled, value);
        }

        private string _appDescription;
        public string AppDescription
        {
            get => _appDescription;
            set => Set(ref _appDescription, value);
        }

        public ICommand SwitchThemeCommand { get; private set; }

        public BasicSettingsPageViewModel()
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
    }
}
