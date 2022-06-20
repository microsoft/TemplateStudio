using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Services;
using Param_RootNamespace.ViewModels;

namespace ViewModelTests;

// To learn more about MSTests: https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest

[TestClass]
public class ListDetailsViewModelTests
{
    public ISampleDataService mockSampleDataService;
    public ListDetailsViewModel listDetailsViewModel;

    public ListDetailsViewModelTests()
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
