using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.Options;
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Contracts.Views;
using Param_RootNamespace.Models;

namespace Param_RootNamespace.Views;

public partial class ts.ItemNamePage : Page, INotifyPropertyChanged, INavigationAware
{
    private readonly AppConfig _appConfig;
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly ISystemService _systemService;
    private readonly IApplicationInfoService _applicationInfoService;
    private bool _isInitialized;
    private AppTheme _theme;
    private string _versionDescription;

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

    public ts.ItemNamePage(IOptions<AppConfig> appConfig, IThemeSelectorService themeSelectorService, ISystemService systemService, IApplicationInfoService applicationInfoService)
    {
        _appConfig = appConfig.Value;
        _themeSelectorService = themeSelectorService;
        _systemService = systemService;
        _applicationInfoService = applicationInfoService;
        InitializeComponent();
    }

    public void OnNavigatedTo(object parameter)
    {
        VersionDescription = $"{Properties.Resources.AppDisplayName} - {_applicationInfoService.GetVersion()}";
        Theme = _themeSelectorService.GetCurrentTheme();
        _isInitialized = true;
    }

    public void OnNavigatedFrom()
    {
    }

    private void OnLightChecked(object sender, RoutedEventArgs e)
    {
        if (_isInitialized)
        {
            _themeSelectorService.SetTheme(AppTheme.Light);
        }
    }

    private void OnDarkChecked(object sender, RoutedEventArgs e)
    {
        if (_isInitialized)
        {
            _themeSelectorService.SetTheme(AppTheme.Dark);
        }
    }

    private void OnDefaultChecked(object sender, RoutedEventArgs e)
    {
        if (_isInitialized)
        {
            _themeSelectorService.SetTheme(AppTheme.Default);
        }
    }

    private void OnPrivacyStatementClick(object sender, RoutedEventArgs e)
        => _systemService.OpenInWebBrowser(_appConfig.PrivacyStatement);
}
