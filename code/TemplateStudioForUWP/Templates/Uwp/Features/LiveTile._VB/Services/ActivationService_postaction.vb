'{**
' These code blocks include the LiveTileFeatureService Instance in the method `GetActivationHandlers()`,
' enable the notification queue in the method `InitializeAsync()` and add a sample LiveTile to the method
' `StartupAsync()` in the ActivationService of your project.
'**}

'{[{
Imports Param_RootNamespace.Core.Helpers
'}]}

Namespace Services
    Friend Class ActivationService
        Private Async Function InitializeAsync() As Task
            '{[{
            Await Singleton(Of LiveTileFeatureService).Instance.EnableQueueAsync().ConfigureAwait(False)
            '}]}
        End Function

        Private Async Function StartupAsync() As Task
'^^
'{[{
            Singleton(Of LiveTileFeatureService).Instance.SampleUpdate()
'}]}
'{??{
            Await Task.CompletedTask
'}??}
        End Function

        Private Iterator Function GetActivationHandlers() As IEnumerable(Of ActivationHandler)
'{[{
            yield Singleton(Of LiveTileFeatureService).Instance
'}]}
        End Function
    End Class
End Namespace
