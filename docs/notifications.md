# Notifications on Windows Template Studio

:heavy_exclamation_mark: There is also a version of [this document with code samples in VB.Net](./notifications.vb.md) :heavy_exclamation_mark: |
----------------------------------------------------------------------------------------------------------------------------------------------- |

Windows Template Studio supports three different type of notifications for UWP:
- Toast notifications
- Hub notifications
- Store notifications

## Toast notifications
ToastNotificationService is in change of sending notifications directly from code in the application. The service contains a method `ShowToastNotification` that sends a notification to the Windows action center. The feature code also includes a `ShowToastNotificationSample` class that shows how to create and send a notification from code.

ToastNotificationsService extends `ActivationHandler` to handle application activation from a toast notification. This code is not implemented on the template because the logic is application dependent. The following [ToastNotificationSample](/samples/notifications/ToastNotificationSample) contains an example of one way to handle application activation from a toast notification.
Check out the [activation documentation](activation.md) to learn more about handling app activation.
The relevant parts of the sample app that handle activation are shown below.

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

Full toast notification documentation for UWP [here](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.notifications.toastcontent).

## Hub notifications
HubNotificationsService is in change of configuring the application with the Azure notifications service to allow the application to receive push notifications from a remote service in Azure. The service contains the `InitializeAsync` method that sets up the Hub Notifications. You must specify the hub name and the access signature before start working with Hub Notifications. There is more documentation about how to create and connect an Azure notifications service [here](https://docs.microsoft.com/azure/app-service-mobile/app-service-mobile-windows-store-dotnet-get-started-push).

Toast Notifications sent from Azure notification service should be handled in the same way as locally generated ones. See the above referenced [ToastNotificationSample](/samples/notifications/ToastNotificationSample) for more.

## Store notifications
StoreNotificationsService is in change of configuring the application with the Windows Dev Center notifications service to allow the application to receive push notifications from Windows Dev Center remote service. The service contains the `InitializeAsync` method that sets up the Store Notifications. This feature use the Store API to configure the notifications.

See the official documentation on how to [configure your app for targeted push notifications](https://docs.microsoft.com/windows/uwp/monetize/configure-your-app-to-receive-dev-center-notifications) and how to [send notifications to your app's customers](https://docs.microsoft.com/windows/uwp/publish/send-push-notifications-to-your-apps-customers).

To handle app activation from a Toast Notification that is sent from Windows Dev Center service will need you to add a similar implementation to that detailed above. The key difference is to call `ParseArgumentsAndTrackAppLaunch` to notify the Windows Dev Center that the app was launched in response to a targeted push notification and to get the original arguments. An example of this is shown below.

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
- [Buttons in Toast Notifications](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.notifications.toastbutton)
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
