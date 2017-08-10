# Settings Page (Code Behind)

By default the settings page contains a single boolean setting to track whether the app should be displayed with the Light or Dark theme.

## Add another boolean setting 

Add the following below the `ToggleSwitch` inside the `StackPanel` in **SettingsView.xaml**

```xml
<CheckBox IsChecked="{x:Bind IsAutoErrorReportingEnabled, Mode=OneWay}"
          x:Uid="Settings_EnableAutoErrorReporting"
          Checked="CheckBoxChecked"
          Unchecked="CheckBoxUnchecked"
          Margin="0,8,0,0" />
```

Add an entry to **Strings/en-us/Resources.resw**

Name: Settings_EnableAutoErrorReporting.Content
Value: Automatically report errors

When run it will now look like this:

![](../resources/modifications/Settings_added_checkbox.png)

But if you try and run it now you will get build errors as the code behind file hasn't been updated to add the new property and event handlers.

### Update the code behind file

In **SettingsPage.xaml.cs**, change the `OnNavigatedTo` method to be like this

```csharp
protected override async void OnNavigatedTo(NavigationEventArgs e)
{
    AppDescription = GetAppDescription();
    IsAutoErrorReportingEnabled = await Windows.Storage.ApplicationData.Current.LocalSettings.ReadAsync<bool>(nameof(IsAutoErrorReportingEnabled));
}
```

also add the following to **SettingsPage.xaml.cs**.

```csharp
using {YourAppName}.Helpers;


private bool _isAutoErrorReportingEnabled;
public bool IsAutoErrorReportingEnabled
{
    get { return _isAutoErrorReportingEnabled; }
    set { Set(ref _isAutoErrorReportingEnabled, value); }
}

private async void CheckBoxChecked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
{
        await Windows.Storage.ApplicationData.Current.LocalSettings.SaveAsync(nameof(IsAutoErrorReportingEnabled), true);
}

private async void CheckBoxUnchecked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
{
    await Windows.Storage.ApplicationData.Current.LocalSettings.SaveAsync(nameof(IsAutoErrorReportingEnabled), false);
}
```

### Accessing the setting from elsewhere in the app

If you want to access the property elsewhere in the app, the easiest way to do this is to read the setting directly. The code below reads the value into a variable called `isEnabled` which you can then query as needed.

```csharp
var isEnabled = await Helpers.SettingsStorageExtensions.ReadAsync<bool>(Windows.Storage.ApplicationData.Current.LocalSettings, "IsAutoErrorReportingEnabled");
```
