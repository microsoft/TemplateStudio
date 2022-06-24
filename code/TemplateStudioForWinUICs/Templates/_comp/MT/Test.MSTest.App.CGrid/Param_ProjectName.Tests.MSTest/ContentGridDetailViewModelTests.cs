using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Services;
using Param_RootNamespace.ViewModels;

namespace Param_ProjectName.Tests.MSTest;

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
