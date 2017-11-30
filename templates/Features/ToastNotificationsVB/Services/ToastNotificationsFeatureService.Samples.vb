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

            ' TODO WTS: Check this documentation to know more about Toast Buttons
            ' Documentation: https://developer.microsoft.com/en-us/windows/uwp-community-toolkit/api/microsoft_toolkit_uwp_notifications_toastbutton
            Dim actions = New ToastActionsCustom()
            actions.Buttons.Add(New ToastButton("OK", "ToastButtonActivationArguments") With { .ActivationType = ToastActivationType.Foreground })
            actions.Buttons.Add(New ToastButtonDismiss("Cancel"))

            ' TODO WTS: Check this documentation to know more about the Launch property
            ' Documentation: https://developer.microsoft.com/en-us/windows/uwp-community-toolkit/api/microsoft_toolkit_uwp_notifications_toastcontent
            Dim content = New ToastContent()
            content.Launch = "ToastContentActivationParams"
            content.Visual = visual
            content.Actions = actions

            ' Add the content to the toast
            ' TODO WTS: Get or set the unique identifier of this notification within the notification Group. Max length 16 characters.
            ' Documentation: https://docs.microsoft.com/uwp/api/windows.ui.notifications.toastnotification
            Dim toast = New ToastNotification(content.GetXml()) With { .Tag = "ToastTag" }

            ' And show the toast
            ShowToastNotification(toast)
        End Sub
    End Class
End Namespace
