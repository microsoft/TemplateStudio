Imports Microsoft.Services.Store.Engagement
Imports Param_RootNamespace.Activation

Namespace Services
    Friend Class StoreNotificationsFeatureService
        Inherits ActivationHandler(Of ToastNotificationActivatedEventArgs)

        Public Async Function InitializeAsync() As Task
            Dim engagementManager As StoreServicesEngagementManager = StoreServicesEngagementManager.GetDefault()
            Await engagementManager.RegisterNotificationChannelAsync()
        End Function

        Protected Overrides Async Function HandleInternalAsync(args As ToastNotificationActivatedEventArgs) As Task
            Dim toastActivationArgs = TryCast(args, ToastNotificationActivatedEventArgs)

            Dim engagementManager As StoreServicesEngagementManager = StoreServicesEngagementManager.GetDefault()
            Dim originalArgs As String = engagementManager.ParseArgumentsAndTrackAppLaunch(toastActivationArgs.Argument)

            ' Use the originalArgs variable to access the original arguments passed to the app.

            Await Task.CompletedTask
        End Function
    End Class
End Namespace 
