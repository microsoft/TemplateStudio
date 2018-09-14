# Update Locator in a MVVMLight project
WTS includes a breaking change in MVVMLight projects to allows MultiView in Apps, Locator should be used as a singleton object instead of an application resource. With this change, we ensure that Locator object will not be instanced more than one time.

Add the following changes in your code.

## 1. App.xaml

Remove ViewModels namespace.

```xml
xmlns:vms="using:App426.ViewModels"
```

Remove the ViewModelLocator from Application.Resources ResourceDictionary.

```xml
<vms:ViewModelLocator x:Key="Locator" />
```

## 2. ViewModelLocator.cs

Make ViewModelLocator constructor as private.

```csharp
private ViewModelLocator()
```

Add a singleton static ViewModelLocator before class contructor.

```csharp
private static ViewModelLocator _current;

public static ViewModelLocator Current => _current ?? (_current = new ViewModelLocator());
```

## 3. Changes in all pages
### 3.1 Page XAML file

Remove the DataContext assignment.

```xml
DataContext="{Binding MainViewModel, Source={StaticResource Locator}}"
```

### 3.2 Page Code Behind file

Get ViewModel from Locator singleton instance.

```csharp
private MainViewModel ViewModel
{
    get { return DataContext as MainViewModel; }
}
```