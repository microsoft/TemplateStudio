'{**
' These code blocks include the HubNotificationsFeatureService Instance in the method `GetActivationHandlers()`
' in the ActivationService of your project and add documentation about how to use the HubNotificationsFeatureService.
'**}

'{[{
Imports Param_RootNamespace.Core.Helpers
'}]}

Namespace Services
    Friend Class ActivationService
        Private Async Function StartupAsync() As Task
'^^
'{[{

            ' TODO WTS: Configure and enable Azure Notification Hub integration.
            '  1. Go to the HubNotificationsFeatureService class, in the InitializeAsync() method, provide the Hub Name and DefaultListenSharedAccessSignature.
            '  2. Uncomment the following line (an exception will be thrown if it is executed and the above information is not provided).
            ' Await Singleton(Of HubNotificationsFeatureService).Instance.InitializeAsync().ConfigureAwait(False)
'}]}
'{??{
            Await Task.CompletedTask
'}??}
        End Function

        Private Iterator Function GetActivationHandlers() As IEnumerable(Of ActivationHandler)
'{[{
            yield Singleton(Of HubNotificationsFeatureService).Instance
'}]}
        End Function
    End Class
End Namespace
