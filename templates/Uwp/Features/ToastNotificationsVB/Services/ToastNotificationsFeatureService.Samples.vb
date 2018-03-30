Imports Microsoft.Toolkit.Uwp.Notifications
Imports Windows.UI.Notifications

Namespace Services
    Friend Partial Class ToastNotificationsFeatureService
        Public Sub ShowToastNotificationSample()
            ' Create the toast content
            Dim visual = New ToastVisual
            visual.BindingGeneric = New ToastBindingGeneric
            visual.BindingGeneric.Children.Add(New AdaptiveText() With { .Text = "Sample Toast Notification" })
            visual.BindingGeneric.Children.Add(New AdaptiveText() With { .Text = "Click OK to see how activation from a toast notification can be handled in the ToastNotificationService." })

            ' More about Toast Buttons at https://developer.microsoft.com/en-us/windows/uwp-community-toolkit/api/microsoft_toolkit_uwp_notifications_toastbutton
            Dim actions = New ToastActionsCustom()
            actions.Buttons.Add(New ToastButton("OK", "ToastButtonActivationArguments") With { .ActivationType = ToastActivationType.Foreground })
            actions.Buttons.Add(New ToastButtonDismiss("Cancel"))

            ' More about the Launch property at https://developer.microsoft.com/en-us/windows/uwp-community-toolkit/api/microsoft_toolkit_uwp_notifications_toastcontent
            Dim content = New ToastContent()
            content.Launch = "ToastContentActivationParams"
            content.Visual = visual
            content.Actions = actions

            ' Add the content to the toast
            ' TODO WTS: Set a unique identifier for this notification within the notification group. (optional)
            ' More details at https://docs.microsoft.com/en-gb/uwp/api/windows.ui.notifications.toastnotification#Windows_UI_Notifications_ToastNotification_Tag
            Dim toast = New ToastNotification(content.GetXml()) With { .Tag = "ToastTag" }

            ' And show the toast
            ShowToastNotification(toast)
        End Sub
    End Class
End Namespace
