Imports Windows.ApplicationModel.Background

Imports Param_RootNamespace.Activation
Imports Param_RootNamespace.BackgroundTasks
Imports Param_RootNamespace.Core.Helpers

Namespace Services
    Friend Class BackgroundTaskService
        Inherits ActivationHandler(Of BackgroundActivatedEventArgs)

        Public Shared ReadOnly Property BackgroundTasks As IEnumerable(Of BackgroundTask)
            Get
                Return BackgroundTaskInstances
            End Get
        End Property

        Private Shared ReadOnly BackgroundTaskInstances As IEnumerable(Of BackgroundTask) = CreateInstances()

        Public Async Function RegisterBackgroundTasksAsync() As Task
            BackgroundExecutionManager.RemoveAccess()
            Dim result = Await BackgroundExecutionManager.RequestAccessAsync()

            If result = BackgroundAccessStatus.DeniedBySystemPolicy OrElse result = BackgroundAccessStatus.DeniedByUser Then
                Return
            End If

            For Each task As BackgroundTask In BackgroundTasks
                task.Register()
            Next
        End Function

        Public Shared Function GetBackgroundTasksRegistration(Of T As BackgroundTask)() As BackgroundTaskRegistration
            If Not BackgroundTaskRegistration.AllTasks.Any(Function(t1) t1.Value.Name = GetType(T).Name) Then
                ' This condition should not be met. If it is it means the background task was not registered correctly.
                ' Please check CreateInstances to see if the background task was properly added to the BackgroundTasks property.
                Return Nothing
            End If

            Return DirectCast(BackgroundTaskRegistration.AllTasks.FirstOrDefault(Function(t2) t2.Value.Name = GetType(T).Name).Value, BackgroundTaskRegistration)
        End Function

        Public Sub Start(taskInstance As IBackgroundTaskInstance)
            Dim task As BackgroundTask = BackgroundTasks.FirstOrDefault(Function(b) b.Match(taskInstance?.Task?.Name))

            If task Is Nothing Then
                ' This condition should not be met. It is it it means the background task to start was not found in the background tasks managed by this service.
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
            Dim backgroundTasks As List(Of BackgroundTask) = New List(Of BackgroundTask)()

            Return backgroundTasks
        End Function
    End Class
End Namespace
