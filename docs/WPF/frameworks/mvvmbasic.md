# MVVM Basic

**Notice: MVVM Basic has been superseded by the [MVVM Toolkit](./mvvmtoolkit.md) and will be removed as an option in a future version of Windows Template Studio.**

MVVM Basic is not a framework but provides the minimum functionality necessary to create an app using the [Model-View-ViewModel (MVVM) pattern](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93viewmodel). It is unique to projects generated with Windows Template Studio and was created for people who can't or don't want to use a 3rd party MVVM Framework such as MVVM Light or Prism.

MVVM Basic is not intended to be a fully featured MVVM Framework and does not include some features that other frameworks do as for example messaging.

MVVM Basic can also serve as a basis for developers who want to create their own MVVM implementation. By providing only the most basic of extra functionality but still following common conventions it should be the easiest option if you want to modify the generated code to meet your preferred way of working.

MVVM Basic uses the `Microsoft.Extensions.Hosting` NuGet Package as application host to provide configuration and dependency injection. For more information see [.NET Generic Host documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host).

## Core files

Projects created with MVVM Basic contain four important classes:

- `Observable`
- `RelayCommand`
- `INavigationAware`
- `ApplicationHostService`

`Observable` contains an implementation of the `INotifyPropertyChanged` interface and is used as a base class for all ViewModels. This makes it easy to update bound properties on the View.

`RelayCommand` contains an implementation of the `ICommand` interface and allows the **View** to call commands on the **ViewModel**, rather than handle UI events directly. You can see examples of `RelayCommand` in use in a generated project.

`INavigationAware` is an interface to be implemented on **ViewModels**. It provides you with the navigation events **OnNavigatedTo** and **OnNavigatedFrom**.

`ApplicationHostService` is the startup service of the app, it's created on App.xaml.cs file and handles the app initialization and the main window creation.

## Working with dependency injection

MVVMBasic projects use dependency injection (DI) to register and consume object instances in the App (Views, ViewModels, Services, etc.).

All classes you want to use with DI must be registered in the ConfigureServices method of App.xaml.cs file.

```csharp
private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
    // Use singleton if you want to share the instance across your app
    services.AddSingleton<IMyService, MyService>();

    // Use transient if you want to obtain new instances
    services.AddTransient<IMyService, MyService>();
}
```

Once these services are registered you can retrieve instances in any class constructor, like for example a ViewModel.

```csharp
public class MainViewModel : Observable
{
    private readonly INavigationService _navigationService;

    public MainViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }
}
```

## Navigation

MVVM Basic assumes **ViewModel-based navigation**. This means that a ViewModel will trigger navigation to another ViewModel. This should be done by calling the `NavigateTo` method on the `INavigationService` and passing the type **full name** of the page's **view-model** you wish to navigate to. In order to resolve the navigation, there is a 1 to 1 relationship between the page and view-model and it's configured in the `PageService` using the `Configure` method.

**Page configuration code**

```csharp
Configure<MainViewModel, MainPage>();
```

**ViewModel navigation code**

```csharp
_navigationService.NavigateTo(typeof(MainViewModel).FullName);
```

**Reacting to navigation events**

ViewModels can react to the navigation events implementing the `INavigationAware` and use the `OnNavigatedTo` to initialize data (i.e. SettingsViewModel). On this method, you can also read the navigation parameter.

```csharp
public void OnNavigatedTo(object parameter)
{
    VersionDescription = $"AppName - {_applicationInfoService.GetVersion()}";
    Theme = _themeSelectorService.GetCurrentTheme();
}
```
