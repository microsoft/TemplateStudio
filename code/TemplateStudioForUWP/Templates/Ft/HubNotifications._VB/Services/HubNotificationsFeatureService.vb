Imports Microsoft.WindowsAzure.Messaging
Imports Windows.Networking.PushNotifications
Imports Param_RootNamespace.Activation

Namespace Services
    ' More about adding push notifications to your Windows app at https://docs.microsoft.com/azure/app-service-mobile/app-service-mobile-windows-store-dotnet-get-started-push
    Friend Class HubNotificationsFeatureService
        Inherits ActivationHandler(Of ToastNotificationActivatedEventArgs)

        Public Async Function InitializeAsync() As Task
            Try
                ' TODO: Set your Hub Name
                Dim hubName = String.Empty

                ' TODO: Set your DefaultListenSharedAccessSignature
                Dim accessSignature = String.Empty

                Dim channel = Await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync()

                Dim hub = New NotificationHub(hubName, accessSignature)
                Dim result = Await hub.RegisterNativeAsync(channel.Uri)

                If result.RegistrationId IsNot Nothing Then
                    ' Registration was successful
                End If

                ' You can also send push notifications from Windows Developer Center targeting your app consumers
                ' More details at https://docs.microsoft.com/windows/uwp/publish/send-push-notifications-to-your-apps-customers
            Catch ex As ArgumentNullException
                ' Until a valid accessSignature and hubName are provided this code will throw an ArgumentNullException.
            Catch ex As Exception
                ' TODO: Channel registration call can fail, please handle exceptions as appropriate to your scenario.
            End Try
        End Function

        Protected Overrides Async Function HandleInternalAsync(args As ToastNotificationActivatedEventArgs) As Task
            ' TODO: Handle activation from toast notification,
            ' Be sure to use the template 'ToastGeneric' in the toast notification configuration XML
            ' to ensure OnActivated is called when launching from a Toast Notification sent from Azure
            '
            ' More about handling activation at https://docs.microsoft.com/windows/uwp/design/shell/tiles-and-notifications/send-local-toast
            Await Task.CompletedTask
        End Function
    End Class
End Namespace
