# Toast Notifications

Adaptive and interactive toast notifications let you create flexible pop-up notifications with more content, optional inline images, and optional user interaction. You can use pictures, buttons, text inputs, actions, and more!

---

:heavy_exclamation_mark: There is also a version of [this document with code samples in C#](./toast-notifications.md) :heavy_exclamation_mark: |
---------------------------------------------------------------------------------------------------------------------------------------- |

The `ToastNotificationService` is in charge of sending notifications directly from code in the application. The service contains a method `ShowToastNotification()` that sends a notification to the Windows action center. The code in this feature also includes a `ShowToastNotificationSample` class that shows how to create and send a notification from code.

`ToastNotificationsService` extends `ActivationHandler` to handle application activation from a toast notification. This code is not implemented on the template because the logic is application dependent. The following contains an example of one way to handle application activation from a toast notification.
Check out the [activation documentation](../activation.vb.md) to learn more about handling app activation.
The relevant parts of the sample app that handle activation are shown below.

```vb
' ToastNotificationService.vb
Protected Overrides Async Function HandleInternalAsync(ByVal args As ToastNotificationActivatedEventArgs) As Task
    // Handle the app activation from a ToastNotification
    NavigationService.Navigate(Of Views.ActivatedFromToastPage)(args)
    Await Task.CompletedTask
End Function

' ActivatedFromToastPage.xaml.vb
Protected Overrides Sub OnNavigatedTo(ByVal e As NavigationEventArgs)
    MyBase.OnNavigatedTo(e)
    ViewModel.Initialize(TryCast(e.Parameter, ToastNotificationActivatedEventArgs))
End Sub

' ActivatedFromToastViewModel.vb
Public Sub Initialize(ByVal args As ToastNotificationActivatedEventArgs)
    ' Check args looking for information about the toast notification
    If args.Argument = "ToastButtonActivationArguments" Then
        ' ToastNotification was clicked on OK Button
        ActivationSource = "ActivationSourceButtonOk".GetLocalized()
    ElseIf args.Argument = "ToastContentActivationParams" Then
        ' ToastNotification was clicked on main content
        ActivationSource = "ActivationSourceContent".GetLocalized()
    End If
End Sub
```

## More information

[Find out more about toast notifications.](https://docs.microsoft.com/windows/uwp/controls-and-patterns/tiles-and-notifications-adaptive-interactive-toasts)

Full documentation of the [ToastContent class](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.notifications.toastcontent).

Use the [visualization app](https://docs.microsoft.com/windows/uwp/controls-and-patterns/tiles-and-notifications-notifications-visualizer) to help design adaptive live tiles and notifications.

[Other useful information about notifications](../notifications.vb.md#other-useful-links-about-notifications)
