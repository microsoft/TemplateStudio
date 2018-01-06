'{**
' These code blocks include the BackgroundTaskService Instance in the method `GetActivationHandlers()`
' and background task registration to the method `InitializeAsync()` in the ActivationService of your project.
'**}

'{[{
Imports Param_RootNamespace.Helpers
'}]}

Namespace Services
    Friend Class ActivationService
        Private Async Function InitializeAsync() As Task
            '{[{
            Singleton(Of BackgroundTaskService).Instance.RegisterBackgroundTasks()
            '}]}
        End Function

        Private Iterator Function GetActivationHandlers() As IEnumerable(Of ActivationHandler)
            '{[{
            yield Singleton(Of BackgroundTaskService).Instance
            '}]}
'{--{

            Exit Function'}--}
        End Function
    End Class
End Namespace 
