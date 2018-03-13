'{[{
Imports System.Reflection
'}]}
Namespace Services
    Friend Class SuspendAndResumeService
        Inherits ActivationHandler(Of LaunchActivatedEventArgs)

        Private Async Function RestoreStateAsync() As Task
            '^^
            '{[{
            If saveState?.Target IsNot Nothing AndAlso GetType(Page).IsAssignableFrom(saveState.Target) Then
                NavigationService.Navigate(saveState.Target, saveState.SuspensionState)
            End If
            '}]}
        End Function
    End Class
End Namespace
