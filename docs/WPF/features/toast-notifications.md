# Toast Notifications

Adaptive and interactive toast notifications let you create flexible pop-up notifications with more content, optional inline images, and optional user interaction. You can use pictures, buttons, text inputs, actions, and more!

The `ToastNotificationsService` is in charge of sending notifications directly from code in the application. The service contains a method `ShowToastNotification` that sends a notification to the Windows action center. The code in this feature also includes a `ShowToastNotificationSample` class that shows how to create and send a notification from code.

`ToastNotificationActivationHandler` extends `IActivationHandler` to handle application activation from a toast notification. This code is not implemented on the template because the logic is application dependent. The following contains an example of one way to handle application activation from a toast notification.

The relevant parts of the sample app that handle activation are shown below.

```csharp
// ToastNotificationActivationHandler.cs
protected override async Task HandleInternalAsync(ToastNotificationActivatedEventArgs args)
{
    // Handle the app activation from a ToastNotification
    NavigationService.Navigate<Views.ActivatedFromToastPage>(args);
    await Task.CompletedTask;
}

// ActivatedFromToastPage.xaml.cs
protected override void OnNavigatedTo(NavigationEventArgs e)
{
    base.OnNavigatedTo(e);
    ViewModel.Initialize(e.Parameter as ToastNotificationActivatedEventArgs);
}

// ActivatedFromToastViewModel.cs
public void Initialize(ToastNotificationActivatedEventArgs args)
{
    // Check args looking for information about the toast notification
    if (args.Argument == "ToastButtonActivationArguments")
    {
        // ToastNotification was clicked on OK Button
        ActivationSource = "ActivationSourceButtonOk".GetLocalized();
    }
    else if(args.Argument == "ToastContentActivationParams")
    {
        // ToastNotification was clicked on main content
        ActivationSource = "ActivationSourceContent".GetLocalized();
    }
}
```

## More information

[Find out more about toast notifications.](https://docs.microsoft.com/windows/uwp/controls-and-patterns/tiles-and-notifications-adaptive-interactive-toasts)

Full documentation of the [ToastContent class](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.notifications.toastcontent).

Use the [visualization app](https://docs.microsoft.com/windows/uwp/controls-and-patterns/tiles-and-notifications-notifications-visualizer) to help design adaptive live tiles and notifications.

[Other useful information about notifications](../notifications.md#other-useful-links-about-notifications)
