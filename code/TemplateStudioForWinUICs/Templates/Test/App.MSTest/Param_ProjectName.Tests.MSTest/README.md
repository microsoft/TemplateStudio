﻿*Recommended Markdown Viewer: [Markdown Editor](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.MarkdownEditor2)*

## Getting Started

[Get started with unit testing](https://docs.microsoft.com/visualstudio/test/getting-started-with-unit-testing?view=vs-2022&tabs=dotnet%2Cmstest), [Use the MSTest framework in unit tests](https://docs.microsoft.com/visualstudio/test/using-microsoft-visualstudio-testtools-unittesting-members-in-unit-tests), and [Run unit tests with Test Explorer](https://docs.microsoft.com/visualstudio/test/run-unit-tests-with-test-explorer) provide an overview of the MSTest framework and Test Explorer.

## Dependency Injection and Mocking

Template Studio uses [dependency injection](https://docs.microsoft.com/dotnet/core/extensions/dependency-injection) which means class dependencies implement interfaces and those dependencies are injected via class constructors.

One of the many benefits of this approach is improved testability, since tests can produce mock implementations of the interfaces and pass them into the object being tested, isolating the object being tested from its dependencies. To mock an interface, create a class that implements the interface, create stub implementations of the interface members, then pass an instance of the class into the object constructor.

## Example

The below example demonstrates testing the ViewModel for the Settings page. `SettingsViewModel` depends on `IThemeSelectorService`, so a `MockThemeSelectorService` class is introduced that implements the interface with stub implementations, and then an instance of that class is passed into the `SettingsViewModel` constructor. The `VerifyVersionDescription` test then validates that the `VersionDescription` property of the `SettingsViewModel` returns the expected value.

```csharp
// SettingsViewModelTests.cs

[TestClass]
public class SettingsViewModelTests
{
    private readonly SettingsViewModel _viewModel;

    public SettingsViewModelTests()
    {
        _viewModel = new SettingsViewModel(new MockThemeSelectorService());
    }

    [TestMethod]
    public void VerifyVersionDescription()
    {
        Assert.AreEqual("App1 - 1.0.0.0", _viewModel.VersionDescription);
    }
}
```

```csharp
// Mocks/MockThemeSelectorService.cs

internal class MockThemeSelectorService : IThemeSelectorService
{
    public ElementTheme Theme => ElementTheme.Default;

    public Task InitializeAsync() => Task.CompletedTask;

    public Task SetRequestedThemeAsync() => Task.CompletedTask;

    public Task SetThemeAsync(ElementTheme theme) => Task.CompletedTask;
}
```
