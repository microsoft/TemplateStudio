using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Services;
using Param_RootNamespace.Services;
using Param_RootNamespace.ViewModels;

namespace Param_ProjectName.Tests.MSTest;

// To learn more about MSTests: https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest

[TestClass]
public class ContentGridParam_RootNamespace
{
    public ISampleDataService mockSampleDataService;
    public IPageService mockPageService;
    public INavigationService mockNavigationService;
    public ContentGridViewModel contentGridViewModel;

    public ContentGridParam_RootNamespace()
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
