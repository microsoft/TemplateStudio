# Dev Center Notifications

Engaging with your customers at the right time and with the right message is key to your success as an app developer. Windows Dev Center provides a data-driven customer engagement platform you can use to send notifications to all of your customers, or targeted to a subset who meet the criteria you've defined in a customer segment.

---

:heavy_exclamation_mark: There is also a version of [this document with code samples in C#](./dev-center-notifications.md) :heavy_exclamation_mark: |
---------------------------------------------------------------------------------------------------------------------------------------- |

The `StoreNotificationsService` is in charge of configuring the application with the Windows Dev Center notifications service to allow the application to receive push notifications from the Windows Dev Center remote service. The service contains the `InitializeAsync()` method that sets up the Store Notifications. This feature uses the Store API to configure the notifications.

See the official documentation on how to [configure your app for targeted push notifications](https://docs.microsoft.com/windows/uwp/monetize/configure-your-app-to-receive-dev-center-notifications) and how to [send notifications to your app's customers](https://docs.microsoft.com/windows/uwp/publish/send-push-notifications-to-your-apps-customers).

To handle app activation from a Notification that is sent from Windows Dev Center service will need you to add a similar implementation to that detailed for [toast notifications](./toast-notifications.md). The key difference is to call `ParseArgumentsAndTrackAppLaunch` to notify the Windows Dev Center that the app was launched in response to a targeted push notification and to get the original arguments. An example of this is shown below.

```vb
Protected Overrides Async Function HandleInternalAsync(ByVal args As ToastNotificationActivatedEventArgs) As Task
    Dim toastActivationArgs = TryCast(args, ToastNotificationActivatedEventArgs)

    Dim engagementManager As StoreServicesEngagementManager = StoreServicesEngagementManager.GetDefault()
    Dim originalArgs As String = engagementManager.ParseArgumentsAndTrackAppLaunch(toastActivationArgs.Argument)

    ' Use the originalArgs variable to access the original arguments passed to the app.
    NavigationService.Navigate(Of Views.ActivatedFromStorePage)(originalArgs)

    Await Task.CompletedTask
End Function
```

---

[Other useful information about notifications](../notifications.vb.md#other-useful-links-about-notifications)
