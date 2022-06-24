using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Models;
using Param_RootNamespace.Core.Services;
using Param_RootNamespace.ViewModels;

namespace Param_ProjectName.Tests.MSTest;

// To learn more about MSTests: https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest

[TestClass]
public class DataGridViewModelTests
{
    public ISampleDataService mockSampleDataService;
    public DataGridViewModel dataGridViewModel;
    public Task<IEnumerable<SampleOrder>> data;

    public DataGridViewModelTests()
    {
        mockSampleDataService = new MockSampleDataService(); // Create your own mock service
        dataGridViewModel = new DataGridViewModel(mockSampleDataService); // Mock service can be injected into the DataGridViewModel
        data = mockSampleDataService.GetGridDataAsync(); // Obtaining data straight from the new mock service
    }

    [TestMethod]
    public void OnNavigatedToTest()
    {
        dataGridViewModel.OnNavigatedTo(null); // OnNavigatedTo sets the DataGridViewModel's source to the mock service's data
        Assert.AreNotEqual(dataGridViewModel.Source, null, "The DataGridViewModel's source was not updated."); // Checks that the DataGridViewModel's source was actually updated
    }

    [TestMethod]
    public void OnNavigatedToCountTest()
    {
        dataGridViewModel.OnNavigatedTo(null);
        Assert.AreEqual(data.Result.Count(), dataGridViewModel.Source.Count(), "The DataGridViewModel's source does not have the correct number of items.");
        // Checks that the DataGridViewModel's source has the correct amount of items
    }

    [TestMethod]
    public void OnNavigatedToItemsTest()
    {
        dataGridViewModel.OnNavigatedTo(null);
        CollectionAssert.AreEqual(dataGridViewModel.Source, data.Result.ToArray());
    }
    //TODO: Add your own tests!
}
