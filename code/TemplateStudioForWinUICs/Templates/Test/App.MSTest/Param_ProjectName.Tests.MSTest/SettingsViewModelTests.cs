using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Services;
using Param_RootNamespace.ViewModels;

namespace Param_RootNamespace;

// To learn more about MSTests: https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest

[TestClass]
public class SettingsParam_RootNamespace
{
    public ILocalSettingsService mockLocalSettingsService;
    public IThemeSelectorService mockThemeSelectorService;
    public SettingsViewModel settingsViewModel;

    public SettingsParam_RootNamespace()
    {
        mockLocalSettingsService = new MockLocalSettingsService();
        mockThemeSelectorService = new MockThemeSelectorService(mockLocalSettingsService);
        settingsViewModel = new SettingsViewModel(mockThemeSelectorService);
    }

    [TestMethod]
    public void TestMethod1()
    {
        //TODO: Add your own tests!
    }   
}
