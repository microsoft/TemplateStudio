'{**
' These code blocks include the HubNotificationsFeatureService Instance in the method `GetActivationHandlers()`
' in the ActivationService of your project and add documentation about how to use the HubNotificationsFeatureService.
'**}
'{[{
Imports Param_RootNamespace.Helpers
'}]}
Namespace Services
    Friend Class ActivationService
        Private Function StartupAsync() As Task
            '{[{
            ' TODO WTS: To use the HubNotificationService specific data related with your Azure Notification Hubs is required.
            '  1. Go to the HubNotificationsFeatureService class, in the InitializeAsync() method, provide the Hub Name and DefaultListenSharedAccessSignature.
            '  2. Uncomment the following line (an exception is thrown if it is executed before the previous information is provided).
            ' Singleton<HubNotificationsFeatureService>.Instance.InitializeAsync();
            '}]}
        End Function

        Private Iterator Function GetActivationHandlers() As IEnumerable(Of ActivationHandler)
            '{[{
            Yield Singleton(Of HubNotificationsFeatureService).Instance
            '}]}
            '{--{
            Return
            '}--}
        End Function
    End Class
End Namespace
