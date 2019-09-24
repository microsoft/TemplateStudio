'{**
' These code blocks include the StoreNotificationsFeatureService Instance in the method `GetActivationHandlers()`
' and initializes it in the method `InitializeAsync()` in the ActivationService of your project.
'**}

'{[{
Imports Param_RootNamespace.Core.Helpers
'}]}

Namespace Services
    Friend Class ActivationService
        Private Async Function StartupAsync() As Task
'^^
'{[{
            Await Singleton(Of StoreNotificationsFeatureService).Instance.InitializeAsync().ConfigureAwait(False)
'}]}
'{??{
            Await Task.CompletedTask
'}??}
        End Function

        Private Iterator Function GetActivationHandlers() As IEnumerable(Of ActivationHandler)
'{[{
            yield Singleton(Of StoreNotificationsFeatureService).Instance
'}]}
        End Function
    End Class
End Namespace
