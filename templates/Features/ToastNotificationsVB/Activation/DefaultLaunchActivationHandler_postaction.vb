'{**
'This code block adds code to show a sample toast notification on application start to your project.
'**}
'{[{
Imports Param_RootNamespace.Helpers
Imports Param_RootNamespace.Services
'}]}

Namespace Activation
    Friend Class DefaultLaunchActivationHandler
        Inherits ActivationHandler(Of LaunchActivatedEventArgs)

        Protected Overrides Async Function HandleInternalAsync(args As LaunchActivatedEventArgs) As Task
            '^^
            '{[{
            ' TODO WTS: This is a sample on how to show a toast notification.
            ' You can use this sample to create toast notifications where needed in your app.
            Singleton(Of ToastNotificationsFeatureService).Instance.ShowToastNotificationSample()
            '}]}
            Await Task.CompletedTask
        End Function
    End Class
End Namespace 
