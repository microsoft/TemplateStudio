using MSTestsRef.Core.Contracts.Services;
using MSTestsRef.Core.Services;
using MSTestsRef.ViewModels;

namespace ViewModelTests;

// To learn more about MSTests: https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest

[TestClass]
public class ContentGridDetailViewModelTests
{
    public ISampleDataService mockSampleDataService;
    public ContentGridDetailViewModel contentGridDetailViewModel;

    public ContentGridDetailViewModelTests() 
    {
        mockSampleDataService = new MockSampleDataService();
        contentGridDetailViewModel = new ContentGridDetailViewModel(mockSampleDataService);
    }

    [TestMethod]
    public void TestMethod1()
    {
        //TODO: Add your own tests!
    }   
}