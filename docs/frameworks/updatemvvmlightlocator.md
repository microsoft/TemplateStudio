# Update Locator in a MVVMLight project

:heavy_exclamation_mark: There is also a version of [this document with code samples in VB.Net](./updatemvvmlightlocator-vb.md) :heavy_exclamation_mark: |

We have changed the WinTS ViewModelLocator implementation for MVVMLight to be able to support MultiView in apps. To avoid the ViewModelLocator from be instantiated more than once, we will used it as as a singleton instead of an application resource. 

To adjust your code, please follow these steps:

## 1. Updates in **App.xaml**

Remove the ViewModels namespace:

```xml
xmlns:vms="using:YourAppName.ViewModels"
```

Remove the ViewModelLocator from the Application.Resources ResourceDictionary:

```xml
<vms:ViewModelLocator x:Key="Locator" />
```

## 2. Updates in **ViewModels\ViewModelLocator.cs**

Change the ViewModelLocator constructor to be private:

```csharp
private ViewModelLocator()
```

Add a static property that contains the current ViewModelLocator above the class contructor:

```csharp
private static ViewModelLocator _current;

public static ViewModelLocator Current => _current ?? (_current = new ViewModelLocator());
```

## 3. Updates in **Services\ActivationService.cs**

Change the way to get the NavigationService from the ViewModelLocator:

Remove the following code:
```csharp
private static ViewModels.ViewModelLocator Locator => Application.Current.Resources["Locator"] as ViewModels.ViewModelLocator;

private static NavigationServiceEx NavigationService => Locator.NavigationService;
```

Add the following code:
```csharp
private static NavigationServiceEx NavigationService => ViewModels.ViewModelLocator.Current.NavigationService;
```

## 4. Changes in all pages
As the ViewModelLocator is no longer part of the ApplicationResources, you will have to change the way in which the pages obtain the ViewModel.

### 4.1 Page XAML files

Remove the **DataContext** assignment.

```xml
DataContext="{Binding YourPageViewModel, Source={StaticResource Locator}}"
```

### 4.2 Page Code Behind files

Get the ViewModel from the ViewModelLocator **singleton** instance.

Remove the following code: 
```csharp
private YourPageViewModel ViewModel
{
    get { return DataContext as YourPageViewModel; }
}
```

Add the following code:
```csharp
private YourPageViewModel ViewModel
{
    get { return ViewModelLocator.Current.YourPageViewModel; }
}
```
