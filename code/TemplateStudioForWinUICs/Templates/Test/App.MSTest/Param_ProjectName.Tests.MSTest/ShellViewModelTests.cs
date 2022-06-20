using MSTestsRef.Contracts.Services;
using MSTestsRef.Services;
using MSTestsRef.ViewModels;

namespace ViewModelTests;

// To learn more about MSTests: https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest

[TestClass]
public class ShellViewModelTests
{
    public IPageService mockPageService;
    public INavigationService mockNavigationService;
    public INavigationViewService mockNavigationViewService;
    public ShellViewModel shellViewModel;

    public ShellViewModelTests()
    {
        mockPageService = new MockPageService();
        mockNavigationService = new MockNavigationService(mockPageService);
        mockNavigationViewService = new MockNavigationViewService(mockNavigationService, mockPageService);
        shellViewModel = new ShellViewModel(mockNavigationService, mockNavigationViewService);
    }

    [TestMethod]
    public void TestMethod1()
    {
        //TODO: Add your own tests!
    }   
}