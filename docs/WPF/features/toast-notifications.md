# Toast Notifications

Adaptive and interactive toast notifications let you create flexible pop-up notifications with more content, optional inline images, and optional user interaction. You can use pictures, buttons, text inputs, actions, and more!

The `ToastNotificationsService` is in charge of sending notifications directly from code in the application. The service contains a method `ShowToastNotification` that sends a notification to the Windows action center. The code in this feature also includes a `ShowToastNotificationSample` class that shows how to create and send a notification from code.

`ToastNotificationActivationHandler` extends `IActivationHandler` to handle application activation from a toast notification. This code is not implemented on the template because the logic is application dependent. The following contains an example of one way to handle application activation from a toast notification.

```csharp
// ToastNotificationActivationHandler.cs
public async Task HandleAsync()
{
    if (App.Current.Windows.OfType<IShellWindow>().Count() == 0)
    {
        // Here you can get an instance of the ShellWindow and choose navigate
        // to a specific page depending on the toast notification arguments
        var shellWindow = _serviceProvider.GetService(typeof(IShellWindow)) as IShellWindow;
        _navigationService.Initialize(shellWindow.GetNavigationFrame());
        shellWindow.ShowWindow();
    }
    else
    {
        App.Current.MainWindow.Activate();
        if (App.Current.MainWindow.WindowState == WindowState.Minimized)
        {
            App.Current.MainWindow.WindowState = WindowState.Normal;
        }
    }

    _navigationService.NavigateTo(typeof(NotificationsViewModel).FullName, _config[ActivationArguments]);

    await Task.CompletedTask;
}

// ActivatedFromToastViewModel.cs, extending INavigationAware
public void OnNavigatedTo(object parameter)
{
    if (parameter is string args)
    {
        // Check args looking for information about the toast notification
        if (args == "ToastButtonActivationArguments")
        {
            // ToastNotification was clicked on OK Button
            ActivationSource = Resources.ActivationSourceButtonOk;
        }
        else if (args == "ToastContentActivationParams")
        {
            // ToastNotification was clicked on main content
            ActivationSource = Resources.ActivationSourceContent;
        }
    }
}
```

## More information

[Find out more about sending a local toast notification from desktop C# apps](https://docs.microsoft.com/windows/uwp/design/shell/tiles-and-notifications/send-local-toast-desktop?tabs=desktop).

Check the Windows Desktop Notification [Sample](https://github.com/WindowsNotifications/desktop-toasts).

Full documentation of the [ToastContent class](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.notifications.toastcontent).

Use the [visualization app](https://docs.microsoft.com/windows/uwp/controls-and-patterns/tiles-and-notifications-notifications-visualizer) to help design adaptive live tiles and notifications.

[Other useful information about notifications](../notifications.md#other-useful-links-about-notifications).
