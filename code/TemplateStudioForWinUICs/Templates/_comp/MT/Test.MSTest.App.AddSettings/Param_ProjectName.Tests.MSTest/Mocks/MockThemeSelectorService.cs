using Microsoft.UI.Xaml;

using Param_RootNamespace.Contracts.Services;

namespace Param_RootNamespace.Services;

public class MockThemeSelectorService : IThemeSelectorService
{
    public MockThemeSelectorService(ILocalSettingsService localSettingsService)
    {
    }

    public ElementTheme Theme { get; set; } = ElementTheme.Default;

    public async Task InitializeAsync()
    {
    }
    public async Task SetRequestedThemeAsync()
    {
    }
    public async Task SetThemeAsync(ElementTheme theme)
    {
    }
}
