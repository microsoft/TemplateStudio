'{**
'This code block adds code to show a sample toast notification on application start to your project.
'**}
'{[{
Imports Param_RootNamespace.Core.Helpers
Imports Param_RootNamespace.Services
'}]}

Namespace Activation
    Friend Class DefaultActivationHandler
        Inherits ActivationHandler(Of IActivatedEventArgs)

        Protected Overrides Async Function HandleInternalAsync(args As IActivatedEventArgs) As Task
            '^^
            '{[{

            ' TODO: Remove or change this sample which shows a toast notification when the app is launched.
            ' You can use this sample to create toast notifications where needed in your app.
            Singleton(Of ToastNotificationsFeatureService).Instance.ShowToastNotificationSample()
            '}]}
            Await Task.CompletedTask
        End Function
    End Class
End Namespace
