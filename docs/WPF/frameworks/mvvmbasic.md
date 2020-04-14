# MVVM Basic

MVVM Basic is not a framework but provides the minimum functionality necessary to create an app using the [Model-View-ViewModel (MVVM) pattern](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93viewmodel). It is unique to projects generated with Windows Template Studio and was created for people who can't or don't want to use a 3rd party MVVM Framework such as MVVM Light or Prism.

MVVM Basic is not intended to be a fully featured MVVM Framework and does not include some features that other frameworks do as for example messaging.

MVVM Basic can also serve as a basis for developers who want to create their own MVVM implementation. By providing only the most basic of extra functionality but still following common conventions it should be the easiest option if you want to modify the generated code to meet your preferred way of working.

MVVM Basic uses the `Microsoft.Extensions.Hosting` NuGet Package as application host to provide configuration and dependency injection. For more information see [.NET Generic Host documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host).

## Core files

Projects created with MVVM Basic contain two important classes:

- `Observable`
- `RelayCommand`

`Observable` contains an implementation of the `INotifyPropertyChanged` interface and is used as a base class for all ViewModels. This makes it easy to update bound properties on the View.

`RelayCommand` contains an implementation of the `ICommand` interface and allows the **View** to call commands on the **ViewModel**, rather than handle UI events directly. You can see examples of `RelayCommand` in use in a generated project.

## Navigation

MVVM Basic assumes ViewModel-based navigation. This means that a ViewModel will trigger navigation to another ViewModel. This should be done by calling the `NavigateTo` method on the `INavigationService` and passing the type full name of the page's view-model you wish to navigate to. In order to resolve the navigation, there is a 1 to 1 relationship between the page and view-model and it's configured in the `PageService` using the `Configure` method.

**Page configuration code**

```csharp
Configure<MainViewModel, MainPage>();
```

**ViewModel navigation code**

```csharp
_navigationService.NavigateTo(typeof(MainViewModel).FullName);
```

**Passing parameters on the navigation**

When passing values this way, they can be accessed in the `OnNavigatedTo` event of the target viewmodel overriding the INavigationAware interface. OnNavigatedTo receives the parameter and it also can be used to initialize data in the page navigation, this sample shows how it's used in the SettingsViewModel.

```csharp
public void OnNavigatedTo(object parameter)
{
    VersionDescription = $"AppName - {_applicationInfoService.GetVersion()}";
    Theme = _themeSelectorService.GetCurrentTheme();
}
```