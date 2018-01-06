Imports Windows.UI.Notifications

Imports Param_RootNamespace.Activation

Namespace Services
    Friend Partial Class ToastNotificationsFeatureService
        Inherits ActivationHandler(Of ToastNotificationActivatedEventArgs)

        Public Sub ShowToastNotification(toastNotification As ToastNotification)
            ToastNotificationManager.CreateToastNotifier().Show(toastNotification)
        End Sub

        Protected Overrides Async Function HandleInternalAsync(args As ToastNotificationActivatedEventArgs) As Task
            ' TODO WTS: Handle activation from toast notification,
            ' for more info on handling activation see
            ' Documentation: https://blogs.msdn.microsoft.com/tiles_and_toasts/2015/07/08/quickstart-sending-a-local-toast-notification-and-handling-activations-from-it-windows-10/

            Await Task.CompletedTask
        End Function
    End Class
End Namespace 
