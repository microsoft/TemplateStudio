using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Services;
using Param_RootNamespace.ViewModels;

namespace Param_ProjectName.Tests.MSTest;

// To learn more about MSTests: https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest

[TestClass]
public class ListDetailsParam_RootNamespace
{
    public ISampleDataService mockSampleDataService;
    public ListDetailsViewModel listDetailsViewModel;

    public ListDetailsParam_RootNamespace()
    {
        mockSampleDataService = new MockSampleDataService();
        listDetailsViewModel = new ListDetailsViewModel(mockSampleDataService);
    }

    [TestMethod]
    public void TestMethod1()
    {
        //TODO: Add your own tests!
    }   
}
