Imports System.Collections.Generic
Imports System.Linq
Imports System.Threading.Tasks

Imports Windows.ApplicationModel.Activation
Imports Windows.ApplicationModel.Background

Imports Param_RootNamespace.Activation
Imports Param_RootNamespace.BackgroundTasks
Imports Param_RootNamespace.Helpers

Namespace Services
    Friend Class BackgroundTaskService
        Inherits ActivationHandler(Of BackgroundActivatedEventArgs)
        Public Shared ReadOnly Property BackgroundTasks As IEnumerable(Of BackgroundTask)
            Get
                Return BackgroundTaskInstances.Value
            End Get
        End Property

        Private Shared ReadOnly BackgroundTaskInstances As New Lazy(Of IEnumerable(Of BackgroundTask))(Function() CreateInstances())

        Public Sub RegisterBackgroundTasks()
            For Each task In BackgroundTasks
                task.Register()
            Next
        End Sub

        Public Shared Function GetBackgroundTasksRegistration(Of T As BackgroundTask)() As BackgroundTaskRegistration
            If Not BackgroundTaskRegistration.AllTasks.Any(Function(t) t.Value.Name = GetType(T).Name) Then
                ' This condition should not be met, if so it means the background task was not registered correctly.
                ' Please check CreateInstances to see if the background task was properly added to the BackgroundTasks property.
                Return Nothing
            End If

            Return DirectCast(BackgroundTaskRegistration.AllTasks.FirstOrDefault(Function(t) t.Value.Name = GetType(T).Name).Value, BackgroundTaskRegistration)
        End Function

        Public Sub Start(taskInstance As IBackgroundTaskInstance)
            Dim task = BackgroundTasks.FirstOrDefault(Function(b) b.Match(taskInstance?.Task?.Name))

            If task Is Nothing Then
                ' This condition should not be met, if so it means the background task to start was not found in the background tasks managed by this service.
                ' Please check CreateInstances to see if the background task was properly added to the BackgroundTasks property.
                Return
            End If

            task.RunAsync(taskInstance).FireAndForget()
        End Sub

        Protected Overrides Async Function HandleInternalAsync(args As BackgroundActivatedEventArgs) As Task
            Start(args.TaskInstance)

            Await Task.CompletedTask
        End Function

        Private Shared Function CreateInstances() As IEnumerable(Of BackgroundTask)
            Dim backgroundTasks As New List(Of BackgroundTask)

            Return backgroundTasks
        End Function
    End Class
End Namespace
