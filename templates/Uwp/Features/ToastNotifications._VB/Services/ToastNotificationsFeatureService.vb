Imports Windows.UI.Notifications

Imports Param_RootNamespace.Activation

Namespace Services
    Friend Partial Class ToastNotificationsFeatureService
        Inherits ActivationHandler(Of ToastNotificationActivatedEventArgs)

        Public Sub ShowToastNotification(toastNotification As ToastNotification)
            Try
                ToastNotificationManager.CreateToastNotifier().Show(toastNotification)
            Catch ex As Exception
                ' TODO WTS: Adding ToastNotification can fail in rare conditions, please handle exceptions as appropriate to your scenario.
            End Try
        End Sub

        Protected Overrides Async Function HandleInternalAsync(args As ToastNotificationActivatedEventArgs) As Task
            ' TODO WTS: Handle activation from toast notification
            ' More details at https://docs.microsoft.com/windows/uwp/design/shell/tiles-and-notifications/send-local-toast

            Await Task.CompletedTask
        End Function
    End Class
End Namespace
