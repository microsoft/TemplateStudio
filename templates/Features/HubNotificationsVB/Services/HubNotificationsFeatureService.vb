Imports Microsoft.WindowsAzure.Messaging
Imports System.Threading.Tasks
Imports Windows.ApplicationModel.Activation
Imports Windows.Networking.PushNotifications

Imports Param_RootNamespace.Activation

Namespace Services
    Friend Class HubNotificationsFeatureService
        Inherits ActivationHandler(Of ToastNotificationActivatedEventArgs)
        Public Sub InitializeAsync()
            '''/ See more about adding push notifications to your Windows app at
            '''/ https://docs.microsoft.com/azure/app-service-mobile/app-service-mobile-windows-store-dotnet-get-started-push
            ' Use your Hub Name here
            Dim hubName = String.Empty
            ' Use your DefaultListenSharedAccessSignature here
            Dim accessSignature = String.Empty
            Dim channel = Await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync()

			Dim hub = New NotificationHub(hubName, accessSignature)
            Dim result = Await hub.RegisterNativeAsync(channel.Uri)
			' RegistrationID let you know it was successful
			If result.RegistrationId IsNot Nothing Then

            End If
            ' You can also send push notifications from Windows Developer Center targeting your app consumers
            ' Documentation: https://docs.microsoft.com/windows/uwp/publish/send-push-notifications-to-your-apps-customers
        End Sub

        Protected Overrides Function HandleInternalAsync(args As ToastNotificationActivatedEventArgs) As Task
            '''/ TODO WTS: Handle activation from toast notification,
            '''/ For more info handling activation see documentation at
            '''/ https://blogs.msdn.microsoft.com/tiles_and_toasts/2015/07/08/quickstart-sending-a-local-toast-notification-and-handling-activations-from-it-windows-10/

            Await Task.CompletedTask
        End Function
    End Class
End Namespace
