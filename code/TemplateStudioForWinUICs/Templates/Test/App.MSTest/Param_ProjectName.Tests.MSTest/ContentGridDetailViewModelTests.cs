using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Services;
using Param_RootNamespace.ViewModels;

namespace Param_RootNamespace;

// To learn more about MSTests: https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest

[TestClass]
public class ContentGridDetailParam_RootNamespace
{
    public ISampleDataService mockSampleDataService;
    public ContentGridDetailViewModel contentGridDetailViewModel;

    public ContentGridDetailParam_RootNamespace() 
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
