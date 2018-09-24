# Update Locator in a MVVMLight project
WTS has changed the ViewModelLocator implementation for MVVMLight to support MultiView in apps. The ViewModelLocator should be used as a singleton instead of an application resource. With this change, we ensure that the ViewModelLocator is not instantiated more than one time.

To adjust your code, please follow these steps:

## 1. Updates in **App.xaml**

Remove the ViewModels namespace in file.

```xml
xmlns:vms="using:App426.ViewModels"
```

Remove the ViewModelLocator from the Application.Resources ResourceDictionary in.

```xml
<vms:ViewModelLocator x:Key="Locator" />
```

## 2. Updates in **ViewModelLocator.cs**

Change the ViewModelLocator constructor to be private.

```csharp
private ViewModelLocator()
```

Add a static property that contains the current ViewModelLocator above the class contructor.

```csharp
private static ViewModelLocator _current;

public static ViewModelLocator Current => _current ?? (_current = new ViewModelLocator());
```

## 3. Changes in all pages
As the ViewModelLocator is no longer part of the ApplicationResources, you will have to change the way in which the pages obtain the ViewModel.

### 3.1 Page XAML files

Remove the **DataContext** assignment.

```xml
DataContext="{Binding YourPageViewModel, Source={StaticResource Locator}}"
```

### 3.2 Page Code Behind files

Get the ViewModel from the ViewModelLocator **singleton** instance.

```csharp
private YourPageViewModel ViewModel
{
    get { return ViewModelLocator.Current.YourPageViewModel; }
}
```

### 4 Updates in **ActivationService.cs**

Change the way to get NavigationService from ViewModelLocator

Remove
```csharp
private static ViewModels.ViewModelLocator Locator => Application.Current.Resources["Locator"] as ViewModels.ViewModelLocator;

private static NavigationServiceEx NavigationService => Locator.NavigationService;
```

Add
```csharp
private static NavigationServiceEx NavigationService => ViewModelLocator.Current.NavigationService;
```