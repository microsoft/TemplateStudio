using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Input;
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Models;

namespace Param_RootNamespace.ViewModels
{
    public class wts.ItemNameViewModel : System.ComponentModel.INotifyPropertyChanged, INavigationAware
    {
        private readonly AppConfig _config;
        private readonly IThemeSelectorService _themeSelectorService;
        private AppTheme _theme;
        private string _versionDescription;
        private ICommand _setThemeCommand;
        private ICommand _privacyStatementCommand;

        public AppTheme Theme
        {
            get { return _theme; }
            set { Param_Setter(ref _theme, value); }
        }

        public string VersionDescription
        {
            get { return _versionDescription; }
            set { Param_Setter(ref _versionDescription, value); }
        }

        public ICommand SetThemeCommand => _setThemeCommand ?? (_setThemeCommand = new System.Windows.Input.ICommand<string>(OnSetTheme));

        public ICommand PrivacyStatementCommand => _privacyStatementCommand ?? (_privacyStatementCommand = new System.Windows.Input.ICommand(OnPrivacyStatement));

        public wts.ItemNameViewModel(Param_ConfigType config, IThemeSelectorService themeSelectorService)
        {
            _config = Param_ConfigValue;
            _themeSelectorService = themeSelectorService;
        }

        public void OnNavigatedTo(Param_OnNavigatedToParams)
        {
            VersionDescription = GetVersionDescription();
            Theme = _themeSelectorService.GetCurrentTheme();
        }

        public void OnNavigatedFrom(Param_OnNavigatedFromParams)
        {
        }

        private string GetVersionDescription()
        {
            var appName = "Param_ProjectName";
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var versionInfo = FileVersionInfo.GetVersionInfo(assemblyLocation);
            return $"{appName} - {versionInfo.FileVersion}";
        }

        private void OnSetTheme(string themeName)
        {
            var theme = (AppTheme)Enum.Parse(typeof(AppTheme), themeName);
            _themeSelectorService.SetTheme(theme);
        }

        private void OnPrivacyStatement()
        {
            // There is an open Issue on this
            // https://github.com/dotnet/corefx/issues/10361
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = _config.PrivacyStatement,
                UseShellExecute = true
            };
            Process.Start(psi);
        }
    }
}