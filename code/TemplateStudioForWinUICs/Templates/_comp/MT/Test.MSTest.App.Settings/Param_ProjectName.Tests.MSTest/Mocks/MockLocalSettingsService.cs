using Param_RootNamespace.Contracts.Services;

namespace Param_RootNamespace.Services;

public class MockLocalSettingsService : ILocalSettingsService
{
    public async Task<T?> ReadSettingAsync<T>(string key)
    {
        return default;
    }

    public async Task SaveSettingAsync<T>(string key, T value)
    {
    }
}
