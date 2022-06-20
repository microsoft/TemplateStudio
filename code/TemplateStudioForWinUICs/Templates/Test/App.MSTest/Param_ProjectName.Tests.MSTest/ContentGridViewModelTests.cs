using MSTestsRef.Contracts.Services;
using MSTestsRef.Core.Contracts.Services;
using MSTestsRef.Core.Services;
using MSTestsRef.Services;
using MSTestsRef.ViewModels;

namespace ViewModelTests;

// To learn more about MSTests: https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest

[TestClass]
public class ContentGridViewModelTests
{
    public ISampleDataService mockSampleDataService;
    public IPageService mockPageService;
    public INavigationService mockNavigationService;
    public ContentGridViewModel contentGridViewModel;

    public ContentGridViewModelTests()
    {
        mockSampleDataService = new MockSampleDataService();
        mockPageService = new MockPageService();
        mockNavigationService = new MockNavigationService(mockPageService);
        contentGridViewModel = new ContentGridViewModel(mockNavigationService, mockSampleDataService);
    }

    [TestMethod]
    public void TestMethod1()
    {
        //TODO: Add your own tests!
    }   
}