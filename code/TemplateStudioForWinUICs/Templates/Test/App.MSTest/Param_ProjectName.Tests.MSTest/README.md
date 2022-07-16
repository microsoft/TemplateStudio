*Recommended Markdown Viewer: [Markdown Editor](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.MarkdownEditor2)*

## Getting Started

[Get started with unit testing](https://docs.microsoft.com/visualstudio/test/getting-started-with-unit-testing?view=vs-2022&tabs=dotnet%2Cmstest), [Use the MSTest framework in unit tests](https://docs.microsoft.com/visualstudio/test/using-microsoft-visualstudio-testtools-unittesting-members-in-unit-tests), and [Run unit tests with Test Explorer](https://docs.microsoft.com/visualstudio/test/run-unit-tests-with-test-explorer) provide an overview of the MSTest framework and Test Explorer.

## Dependency Injection and Mocking

Template Studio uses [dependency injection](https://docs.microsoft.com/dotnet/core/extensions/dependency-injection) which means class dependencies implement interfaces and those dependencies are injected via class constructors.

One of the many benefits of this approach is improved testability, since tests can produce mock implementations of the interfaces and pass them into the object being tested, isolating the object being tested from its dependencies. To mock an interface, create a class that implements the interface, create stub implementations of the interface members, then pass an instance of the class into the object constructor.

## Example

The below example demonstrates testing the ViewModel for the DataGrid page. `DataGridViewModel` depends on `ISampleDataService`, so a `MockSampleDataService` class is introduced that implements the interface with stub data, and then an instance of that class is passed into the `DataGridViewModel` constructor. The `OnNavigatedToTest` then calls `OnNavigatedTo` on the `DataGridViewModel` and validates that its state is updated appropriately.

```csharp
namespace TestProject1;

[TestClass]
public class DataGridViewModelTests
{
    public ISampleDataService _mockSampleDataService;
    public DataGridViewModel _dataGridViewModel;
    public Task<IEnumerable<SampleOrder>> _data;

    public DataGridViewModelTests()
    {
        _mockSampleDataService = new MockSampleDataService();
        _dataGridViewModel = new DataGridViewModel(_mockSampleDataService);
        _data = _mockSampleDataService.GetGridDataAsync();
    }

    [TestMethod]
    public void OnNavigatedToTest()
    {
        _dataGridViewModel.OnNavigatedTo(null);

        Assert.AreNotEqual(_dataGridViewModel.Source, null, "The DataGridViewModel Source property was not updated.");
        Assert.AreEqual(_data.Result.Count(), _dataGridViewModel.Source.Count(), "The DataGridViewModel Source property does not have the correct number of items.");
        CollectionAssert.AreEqual(_dataGridViewModel.Source, _data.Result.ToArray());
    }
}
```

## Unpackaged Project

In order to access the `.resw` resources used in App7, this line must be added to the test project's `.csproj` file within the first PropertyGroup:

```xaml
	<ProjectPriFileName>resources.pri</ProjectPriFileName>
```
This way, in the case of testing the SettingsViewModel, for example, the test project can successfully create a `SettingsViewModel` object, which uses `App7/Strings/en-us/Resources.resw`.

## MSIX-Packaged Project
unpackaged test project + packaged 
## DataGrid View Model Testing Implementation (Unpackaged)

In the testing file, a public class is created containing an empty test method. Within the TestClass, instances of the DataGridViewModel and the MockSampleDataService can be created. Then, within
a constructor, these can initialized to then be used within the TestMethods. There is also a data variable for the Mock Sample Data Service's contents:


## WORKING NOTES:

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
