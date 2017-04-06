# Settings Page

By default the settings page uses contains a single boolean setting to track whether the app should be displayed with the Light or Dark theme.

You can add additional settings by following the instructions below.

## Add another boolean setting

Let's add a boolean setting to control whether errors should be automatically reported.  
Adding a setting requires you to:

* Update the View so it's possible to see and change the setting
* Update the ViewModel to add logic related to changing the setting.

### Update the View

The default code uses a `ToggleSwitch` to control the theme. It's possible to use another `ToggleSwitch` for our new setting but we'll use a `CheckBox` to show an alternative.

Add the following below the `ToggleSwitch` inside the `StackPanel` in **SettingsView.xaml**.

```xml
<CheckBox IsChecked="{x:Bind ViewModel.IsAutoErrorReportingEnabled, Mode=TwoWay}"
          x:Uid="Settings_EnableAutoErrorReporting"
          Margin="{StaticResource SmallTopMargin}" />
```

When run it will now look like this.

![](resources/modifications/Settings_added_checkbox.png)

But if you try and run it now you will get build errors as the ViewModel hasn't been updated to add the new property.

### Update the ViewModel

There are two ways to do this. 

1. The quick and dirty way (not recommended)
2. Change the ViewModel to be a singleton (recommended)

Note that the `IsLightThemeEnabled` does not suffer the issues affected by 'the quick and dirty way' because it uses a static `ThemeSelectorService`. It is not necessary to create equivalent services for every setting but is appropriate for the theme preference as this is needed when the app launches.

#### The quick and dirty way  (NOT recommended)

Add the following to **SettingsViewModel.cs**

```csharp
private bool? _isAutomaticErrorReportingEnabled;

public bool? IsAutoErrorReportingEnabled
{
    get
    {
        if (_isAutomaticErrorReportingEnabled == null)
        {
            LoadAutoSetting();
        }

        return _isAutomaticErrorReportingEnabled ?? false;
    }

    set
    {
        if (value != _isAutomaticErrorReportingEnabled)
        {
            Task.Run(async () => await ApplicationData.Current.LocalSettings.SaveAsync(nameof(IsAutoErrorReportingEnabled), value ?? false));
        }

        Set(ref _isAutomaticErrorReportingEnabled, value);
    }
}

private async void LoadAutoSetting()
{
    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
        Windows.UI.Core.CoreDispatcherPriority.Normal,
        async () =>
        {
            IsAutoErrorReportingEnabled =
                await ApplicationData.Current.LocalSettings.ReadAsync<bool>(
                    nameof(IsAutoErrorReportingEnabled));
        });
}
```

The reason this is bad is because of its use of `async void` which should really only be used for top level event handling. For more on this see [this MSDN article](https://msdn.microsoft.com/en-us/magazine/jj991977.aspx) or [this Channel9 video](https://channel9.msdn.com/Series/Three-Essential-Tips-for-Async/Tip-1-Async-void-is-for-top-level-event-handlers-only).

#### Change the ViewModel to be a singleton (recommended)

It would be unusual for an app to have a configurable setting that wasn't also used in another part of the app. In this scenario it's important that the same settings values are used in all location. The simplest way to do this is to have a single instance of the `SettingsViewModel` and use it for all access to settings values.

The generated code allows you to create multiple instances of the `SettingsViewModel` but this could potentially lead to a scenario where you had multiple different values for the same setting. Instead we can change things so there's only a single instance of `SettingsViewModel` being used.

Add the following to **SettingsViewModel.cs**

```csharp
private static readonly SettingsViewModel _singletonInstance = new SettingsViewModel();

public static SettingsViewModel GetInstance()
{
    return _singletonInstance;
}

private static bool _hasStaticInstanceBeenInitialized = false;

public static async Task EnsureInstanceInitialized()
{
    if (!_hasStaticInstanceBeenInitialized)
    {
        _hasStaticInstanceBeenInitialized = true;

        _singletonInstance.IsAutoErrorReportingEnabled =
            await ApplicationData.Current.LocalSettings.ReadAsync<bool>(nameof(IsAutoErrorReportingEnabled));

        _singletonInstance.Initialize();
    }
}

private bool? _isAutomaticErrorReportingEnabled;

public bool? IsAutoErrorReportingEnabled
{
    get => _isAutomaticErrorReportingEnabled ?? false;

    set
    {
        if (value != _isAutomaticErrorReportingEnabled)
        {
            Task.Run(async () => await ApplicationData.Current.LocalSettings.SaveAsync(nameof(IsAutoErrorReportingEnabled), value ?? false));
        }

        Set(ref _isAutomaticErrorReportingEnabled, value);
    }
}
```

The will now allow us to reference a single instance of the `SettingsViewModel` class anywhere.  
**SettingsView.xaml.cs** currently creates an instance of the ViewModel so we should change it.  
Change the property declaration from this

```csharp
public SettingsViewModel ViewModel { get; } = new SettingsViewModel();
```

to this

```csharp
public SettingsViewModel ViewModel { get; } = SettingsViewModel.GetInstance();
```
and change the `OnNavigatedTo()` method so that instead of being like

```csharp
protected override void OnNavigatedTo(NavigationEventArgs e)
{
    ViewModel.Initialize();
}
```

It looks like

```csharp
protected override async void OnNavigatedTo(NavigationEventArgs e)
{
    await SettingsViewModel.EnsureInstanceInitialized();
}
```

Then, whenever you want to access something from the ViewModel, anywhere in the app, ensure you have called `await SettingsViewModel.EnsureInstanceInitialized();`. You can get or set the property with `SettingsViewModel.GetInstance().IsAutoErrorReportingEnabled`.  
For example:

```csharp
try
{
    ...
}
catch (Exception exc)
{
    if (SettingsViewModel.GetInstance().IsAutoErrorReportingEnabled)
    {
        // Send the error details to the server 
    }
}
```

If you only use the instance in one or two places you could call `EnsureInstanceInitialized()` each time before you call `GetInstance()`. As `EnsureInstanceInitialized()` only needs to be called once before it is used, if you have lots of settings or need to access them in many places you could call it once as part of the `InitializeAsync()` method in **ActivationService.cs**.

