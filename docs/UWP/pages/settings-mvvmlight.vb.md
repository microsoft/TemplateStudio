# Settings Page (MVVM Light)

:heavy_exclamation_mark: There is also a version of [this document with code samples in C#](./settings-mvvmlight.md) :heavy_exclamation_mark: |
--------------------------------------------------------------------------------------------------------------------------------------------- |

By default the settings page contains a single boolean setting to track whether the app should be displayed with the Light or Dark theme.

## Add another boolean setting

Let's add a boolean setting to control whether errors should be automatically reported.
Adding a setting requires you to:

* Update the View so it's possible to see and change the setting
* Update the ViewModel to add logic related to changing the setting.

### Update the View

Add the following below the `StackPanel` containing the `RadioButton`s in **SettingsView.xaml**

```xml
<CheckBox IsChecked="{x:Bind ViewModel.IsAutoErrorReportingEnabled, Mode=TwoWay}"
          x:Uid="Settings_EnableAutoErrorReporting"
          Margin="0,8,0,0" />
```

Add an entry to **Strings/en-us/Resources.resw**

Name: **Settings_EnableAutoErrorReporting.Content**

Value: **Automatically report errors**

When run it will now look like this:

![Partial screenshot of settings page showing new option](../resources/modifications/Settings_added_checkbox.png)

But if you try and run it now you will get build errors as the ViewModel hasn't been updated to add the new property.

### Update the ViewModel

The `IsLightThemeEnabled` property uses a static `ThemeSelectorService`. It is not necessary to create equivalent services for every setting but is appropriate for the theme preference as this is needed when the app launches.

Because we may want to access settings in various parts of the app it's important that the same settings values are used in all locations. The simplest way to do this is to have a single instance of the `SettingsViewModel` and use it for all access to settings values.

The generated code in includes a Singleton helper class to provide access to a single instance of the view model which we can use everywhere.

With this knowledge we can now add the new property for accessing our stored setting. We also need to add a new, awaitable initializer for the property too.
Add the following to **SettingsViewModel.vb**

```vb
Imports {YourAppName}.Helpers
Imports System.Threading.Tasks

Private _isAutomaticErrorReportingEnabled As Boolean?

Public Property IsAutoErrorReportingEnabled As Boolean?
    Get
        Return If(_isAutomaticErrorReportingEnabled, False)
    End Get

    Set(ByVal value As Boolean?)
        If value <> _isAutomaticErrorReportingEnabled Then
            Task.Run(Async Function() Await Windows.Storage.ApplicationData.Current.LocalSettings.SaveAsync(NameOf(IsAutoErrorReportingEnabled), If(value, False)))
        End If

        [Set](_isAutomaticErrorReportingEnabled, value)
    End Set
End Property

Private _hasInstanceBeenInitialized As Boolean = False

Public Async Function EnsureInstanceInitializedAsync() As Task
    If Not _hasInstanceBeenInitialized Then
        IsAutoErrorReportingEnabled = Await Windows.Storage.ApplicationData.Current.LocalSettings.ReadAsync(Of Boolean)(NameOf(IsAutoErrorReportingEnabled))
        Initialize()
        _hasInstanceBeenInitialized = True
    End If
End Function
```

Then change the `SwitchThemeCommand` property to match this:

```vb
Public Property SwitchThemeCommand As ICommand
    Get
        If _switchThemeCommand Is Nothing Then
            _switchThemeCommand = New RelayCommand(Of ElementTheme)(Async Sub(param)
                If _hasInstanceBeenInitialized Then
                    Await ThemeSelectorService.SetThemeAsync(param)
                End If
            End Sub)
        End If

        Return _switchThemeCommand
    End Get
End Property
```

We must now update our uses of the ViewModel.

### If your app is using the Blank or NavigationView structure

In **SettingsPage.xaml.vb** change the `OnNavigatedTo()` method so that instead of calling the old Initialize method, like this:

```vb
Protected Overrides Sub OnNavigatedTo(ByVal e As NavigationEventArgs)
    ViewModel.Initialize()
End Sub
```

It now awaits the call to the new Initializer like this:

```vb
Protected Overrides Async Sub OnNavigatedTo(ByVal e As NavigationEventArgs)
    Await ViewModel.EnsureInstanceInitializedAsync()
End Sub
```

### If your app is using the 'Pivot and Tabs' structure

In **SettingsPage.xaml.vb** change the constructor so that it handles the `OnLoaded` event and add the following event handler, like this:

```vb
Public Sub SettingsPage()
    InitializeComponent()
    ViewModel.Initialize()
    AddHandler Me.Loaded, AddressOf SettingsPage_Loaded
End Sub

Private Async Sub SettingsPage_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
    RemoveHandler Me.Loaded, AddressOf SettingsPage_Loaded
    Await ViewModel.EnsureInstanceInitializedAsync()
End Sub
```

Everything is now complete and you can run the app and it will remember the value between invocations of the app.

### Accessing the setting from elsewhere in the app

If you want to access the property elsewhere in the app, ensure you have called `await ServiceLocator.Current.GetInstance(Of SettingsViewModel)().EnsureInstanceInitializedAsync();`. Then you can get or set the property with `ServiceLocator.Current.GetInstance(Of SettingsViewModel)().IsAutoErrorReportingEnabled`.
For example:

```vb
Try
    ...
Catch  exc as Exception
    Await ServiceLocator.Current.GetInstance(Of SettingsViewModel)().EnsureInstanceInitializedAsync()
    If ServiceLocator.Current.GetInstance(Of SettingsViewModel)().IsAutoErrorReportingEnabled Then
        ' Send the error details to the server
    End If
End Try
```

If you only use the value in one or two places you could call `EnsureInstanceInitializedAsync()` each time before you access the SettingsViewModel instance. But, as `EnsureInstanceInitializedAsync()` only needs to be called once before it is used, if you have lots of settings or need to access them in many places you could call it once as part of the `InitializeAsync()` method in **ActivationService.vb**.
