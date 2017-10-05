# Notifications on Windows Template Studio

Windows Template Studio supports three different type of notifications for UWP:
- Toast notifications
- Hub notifications
- Store notifications

## Toast notifications
ToastNotificationService is in change of send notifications directly from code in the application. The service contains a method `ShowToastNotification` that sends a notification to the Windows action center. The feature code also includes a ShowToastNotificationSample class that show how to create and send a notification from code.

ToastNotificationsService extends Windows Template Studio ActivationHandler to can handle application activation from toast notification. This code is not implemented on the template because depending on the nature of the application, one notification should be handled or not. The following [ToastNotificationSample](/samples/notifications/ToastNotificationSample) contains the neccesary code to handle application activation from a toast notification.
Check out the [activation documentation](activation.md) to learn more about handling activation in app or checkout.

```csharp
// ToastNotificationService.cs
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

Full toast notification documentation for UWP [here](https://developer.microsoft.com/en-us/windows/uwp-community-toolkit/api/microsoft_toolkit_uwp_notifications_toastcontent).

## Hub notifications
HubNotificationsService is in change of configuring the application with the Azure notifications service to allow the application to receive push notifications from a remote service in Azure. The service contains a `InitializeAsync` method that setups the Hub Notifications. Yo must specify the hub name and the access signature before start working with Hub Notifications. Here is more [documentation](https://docs.microsoft.com/en-us/azure/app-service-mobile/app-service-mobile-windows-store-dotnet-get-started-push) about how to create and connect an Azure notifications service.

About handling the app activation from a Toast Notification that is sent from Azure notification service you will need to add the same implementation of [ToastNotificationSample](/samples/notifications/ToastNotificationSample) detailed above.

## Store notifications
StoreNotificationsService is in change of configuring the application with the Windows Dev Center notifications service to allow the application to receive push notifications from Windows Dev Center remote service. The service contains a `InitializeAsync` method that setups the Store Notifications. This feature use the Store API to configure the notifications.

See more about how to [configure your app for targeted push notifications](https://docs.microsoft.com/windows/uwp/monetize/configure-your-app-to-receive-dev-center-notifications) and how to [send notifications to your app's customers](https://docs.microsoft.com/windows/uwp/publish/send-push-notifications-to-your-apps-customers).

About handling the app activation from a Toast Notification that is sent from Windows Dev Center service you will need to add a similar implementation that detailed above, in this case, we should call to `ParseArgumentsAndTrackAppLaunch` to notify Windows Dev Center that the app was launched in response to a targeted push notification from Dev Center and get the original arguments.

```csharp
protected override async Task HandleInternalAsync(ToastNotificationActivatedEventArgs args)
{
    var toastActivationArgs = args as ToastNotificationActivatedEventArgs;

    StoreServicesEngagementManager engagementManager = StoreServicesEngagementManager.GetDefault();
    string originalArgs = engagementManager.ParseArgumentsAndTrackAppLaunch(toastActivationArgs.Argument);

    //// Use the originalArgs variable to access the original arguments passed to the app.
    NavigationService.Navigate<Views.ActivatedFromStorePage>(originalArgs);

    await Task.CompletedTask;
}
```


Other interesting links about notifications
- [Buttons in Toast Notifications](https://developer.microsoft.com/en-us/windows/uwp-community-toolkit/api/microsoft_toolkit_uwp_notifications_toastbutton)
- [Toast Notification class](https://docs.microsoft.com/uwp/api/windows.ui.notifications.toastnotification)
- [UWP Toast notification when application is in foreground](https://social.msdn.microsoft.com/Forums/en-US/ff8acad4-f0c2-4a36-ac90-84780276fd09/uwptoast-notification-when-application-is-in-foreground)
- [Suppress Toast Notification when app is in the foreground](https://social.msdn.microsoft.com/Forums/en-US/21a374dc-6510-48ea-b058-a9d4424cda4b/uwpc-suppress-toast-notification-when-app-is-in-the-foreground)
- [Windows traffic app sample](https://github.com/microsoft/windows-appsample-trafficapp/)
- [Windows notifications samples](https://github.com/Microsoft/Windows-universal-samples/tree/master/Samples/Notifications)
- [Windows toast notifications sample](https://github.com/WindowsNotifications/quickstart-sending-local-toast-win10)
- [Send push notifications from Windows Developer Center](https://docs.microsoft.com/windows/uwp/publish/send-push-notifications-to-your-apps-customers)
- [Handle toast notification activation](https://blogs.msdn.microsoft.com/tiles_and_toasts/2015/07/08/quickstart-sending-a-local-toast-notification-and-handling-activations-from-it-windows-10/)
- [Adding push notifications](https://docs.microsoft.com/azure/app-service-mobile/app-service-mobile-windows-store-dotnet-get-started-push)
- [Configure your app for targeted push notifications](https://docs.microsoft.com/windows/uwp/monetize/configure-your-app-to-receive-dev-center-notifications)
- [Send notifications to your app's customers](https://docs.microsoft.com/windows/uwp/publish/send-push-notifications-to-your-apps-customers)