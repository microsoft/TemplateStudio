using System;
using System.Windows.Input;

using DotNetCoreWpfApp.Contracts.Services;
using DotNetCoreWpfApp.Contracts.ViewModels;
using DotNetCoreWpfApp.Helpers;
using DotNetCoreWpfApp.Models;

using Microsoft.Extensions.Options;

namespace DotNetCoreWpfApp.ViewModels
{
    // TODO WTS: Change the URL for your privacy policy in the appsettings.json file, currently set to https://YourPrivacyUrlGoesHere
    public class SettingsViewModel : Observable, INavigationAware
    {
        private readonly AppConfig _appConfig;
        private readonly IThemeSelectorService _themeSelectorService;
        private readonly ISystemService _systemService;
        private readonly IApplicationInfoService _applicationInfoService;
        private AppTheme _theme;
        private string _versionDescription;
        private ICommand _setThemeCommand;
        private ICommand _privacyStatementCommand;

        public AppTheme Theme
        {
            get { return _theme; }
            set { Set(ref _theme, value); }
        }

        public string VersionDescription
        {
            get { return _versionDescription; }
            set { Set(ref _versionDescription, value); }
        }

        public ICommand SetThemeCommand => _setThemeCommand ??= new RelayCommand<string>(OnSetTheme);

        public ICommand PrivacyStatementCommand => _privacyStatementCommand ??= new RelayCommand(OnPrivacyStatement);

        public SettingsViewModel(IOptions<AppConfig> appConfig, IThemeSelectorService themeSelectorService, ISystemService systemService, IApplicationInfoService applicationInfoService)
        {
            _appConfig = appConfig.Value;
            _themeSelectorService = themeSelectorService;
            _systemService = systemService;
            _applicationInfoService = applicationInfoService;
        }

        public void OnNavigatedTo(object parameter)
        {
            VersionDescription = $"DotNetCoreWpfApp - {_applicationInfoService.GetVersion()}";
            Theme = _themeSelectorService.GetCurrentTheme();
        }

        public void OnNavigatedFrom()
        {
        }

        private void OnSetTheme(string themeName)
        {
            var theme = (AppTheme)Enum.Parse(typeof(AppTheme), themeName);
            _themeSelectorService.SetTheme(theme);
        }

        private void OnPrivacyStatement()
            => _systemService.OpenInWebBrowser(_appConfig.PrivacyStatement);
    }
}
