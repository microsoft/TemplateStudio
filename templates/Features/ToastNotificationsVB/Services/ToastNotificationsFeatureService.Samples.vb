Imports Microsoft.Toolkit.Uwp.Notifications
Imports Windows.UI.Notifications

Namespace Services
    Partial Friend Class ToastNotificationsFeatureService
        Public Sub ShowToastNotificationSample()
            ' Create the toast content
            ' TODO WTS: Check this documentation to know more about the Launch property
            ' Documentation: https://developer.microsoft.com/en-us/windows/uwp-community-toolkit/api/microsoft_toolkit_uwp_notifications_toastcontent

            ' TODO WTS: Check this documentation to know more about Toast Buttons
            ' Documentation: https://developer.microsoft.com/en-us/windows/uwp-community-toolkit/api/microsoft_toolkit_uwp_notifications_toastbutton

            Dim content = New ToastContent With {
                .Launch = "ToastContentActivationParams",
                .Visual = New ToastVisual With {
                    .BindingGeneric = New ToastBindingGeneric With {
                        .Children = {New AdaptiveText With {
                            .Text = "Sample Toast Notification"
                        }, New AdaptiveText With {
                            .Text = "Click OK to see how activation from a toast notification can be handled in the ToastNotificationService."
                        }}
                    }
                },
                .Actions = New ToastActionsCustom With {
                    .Buttons = {New ToastButton("OK", "ToastButtonActivationArguments") With {
                        .ActivationType = ToastActivationType.Foreground
                    }, New ToastButtonDismiss("Cancel")}
                }
            }

            ' Create the toast
            ' TODO WTS: Gets or sets the unique identifier of this notification within the notification Group. Max length 16 characters.
            ' Documentation: https://docs.microsoft.com/uwp/api/windows.ui.notifications.toastnotification
            Dim toast = New ToastNotification(content.GetXml()) With {
                .Tag = "ToastTag"
            }

            ' And show the toast
            ShowToastNotification(toast)
        End Sub
    End Class
End Namespace
