# MSTests for Template Studio
This guide will demonstrate using dependency injection and service-mocking to test your pages' View Models. The example included here is the DataGridViewModel, which uses the Sample Data Service.

## Mocking Services
The services in this solution implement interfaces. The services can be found within `App7/Services` and `App7.Core/Services`. The corresponding interfaces that they implement are found within
`App7/Contracts/Services` and `App7.Core/Contracts/Services`, respectively. In the case of the DataGrid View Model, custom data can be used for testing by creating a Mock Sample Data Service.
This can be done by copying the existing `SampleDataService.cs` into the test project and renaming it to `MockSampleDataService.cs`. The same can be said for other services in the solution, but it is important to also
create services that they depend on so that they can be initialized correctly. An example of this is the Navigation Service, which implements the INavigationService interface and requires an instance of an IPageService in
its constructor.

## Unpackaged Project
In order to access the `.resw` resources used in App7, this line must be added to the test project's `.csproj` file within the first PropertyGroup:

```xaml
	<ProjectPriFileName>resources.pri</ProjectPriFileName>
```
This way, in the case of testing the SettingsViewModel, for example, the test project can successfully create a `SettingsViewModel` object, which uses `App7/Strings/en-us/Resources.resw`.

## MSIX-Packaged Project

## DataGrid View Model Testing Implementation (Unpackaged)

In the testing file, a public class is created containing an empty test method. Within the TestClass, instances of the DataGridViewModel and the MockSampleDataService can be created. Then, within
a constructor, these can initialized to then be used within the TestMethods. There is also a data variable for the Mock Sample Data Service's contents:

```csharp
namespace TestProject1;

[TestClass]
public class UnitTest1
{
    public ISampleDataService mockSampleDataService;
    public DataGridViewModel dataGridViewModel;
    public Task<IEnumerable<SampleOrder>> data;

    public UnitTest1()
    {
        mockSampleDataService = new MockSampleDataService(); // Create your own mock service
        dataGridViewModel = new DataGridViewModel(mockSampleDataService); // Mock service can be injected into the DataGridViewModel
        data = mockSampleDataService.GetGridDataAsync(); // Obtaining data straight from the new mock service
    }
}
```

For this example specifically, you can test whether or not the DataGrid View Model obtains the correct data from the sample data service. Therefore, the following tests compare the view model's data and the data obtained directly
from the data service.

```csharp
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
        CollectionAssert.AreEqual(dataGridViewModel.Source, data.Result.ToArray()); // Checks that the DataGridViewModel has the correct items
    }
```

## Resources
![Get started with unit testing | Microsoft Docs](https://docs.microsoft.com/en-us/visualstudio/test/getting-started-with-unit-testing?view=vs-2022&tabs=dotnet%2Cmstest)

### WORKING NOTES:

## Unpackaged App
Settings: <ProjectPriFileName>resources.pri</ProjectPriFileName> to the first PropertyGroup
(will be handled by post action)

## Packaged App

    <PackageReference Include="Microsoft.TestPlatform.TestHost">
    <Version>17.2.0</Version>
    <ExcludeAssets>build</ExcludeAssets>
    </PackageReference>

    NEED TO FIND RESOURCE FILE FIX

## something abt making mock empty??
