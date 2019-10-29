# Update Locator in a MVVMLight project

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

## 2. Updates in **ViewModels\ViewModelLocator.vb**

Change the ViewModelLocator constructor to be private:

```vb
Private Sub New()
```

Add a shared property that contains the current ViewModelLocator above the class constructor:

```vb
Private Shared _current As ViewModelLocator

Public Shared ReadOnly Property Current As ViewModelLocator
    Get
        If _current Is Nothing Then
            _current = New ViewModelLocator()
        End If
        Return _current
    End Get
End Property
```

## 3. Updates in **Services\ActivationService.vb**

Change the way to get the NavigationService from the ViewModelLocator:

Remove the following code:

```vb
Private ReadOnly Property Locator As ViewModels.ViewModelLocator
    Get
        Return TryCast(Application.Current.Resources("Locator"), ViewModels.ViewModelLocator)
    End Get
End Property

Private ReadOnly Property NavigationService As NavigationServiceEx
    Get
        Return Locator.NavigationService
    End Get
End Property
```

Add the following code:

```vb
Private ReadOnly Property NavigationService As NavigationServiceEx
    Get
        Return ViewModelLocator.Current.NavigationService
    End Get
End Property
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

```vb
Private ReadOnly Property ViewModel As YourPageViewModel
    Get
        Return TryCast(DataContext, YourPageViewModel)
    End Get
End Property
```

Add the following code:

```vb
Private ReadOnly Property ViewModel As YourPageViewModel
    Get
        Return ViewModelLocator.Current.YourPageViewModel
    End Get
End Property
```
