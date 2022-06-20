using MSTestsRef.Contracts.Services;
using MSTestsRef.Services;
using MSTestsRef.ViewModels;

namespace ViewModelTests;

// To learn more about MSTests: https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest

[TestClass]
public class SettingsViewModelTests
{
    public ILocalSettingsService mockLocalSettingsService;
    public IThemeSelectorService mockThemeSelectorService;
    public SettingsViewModel settingsViewModel;

    public SettingsViewModelTests()
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