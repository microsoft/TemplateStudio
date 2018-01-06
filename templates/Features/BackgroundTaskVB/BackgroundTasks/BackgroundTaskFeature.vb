Imports Windows.ApplicationModel.Background
Imports Windows.System.Threading

Namespace BackgroundTasks
    Public NotInheritable Class BackgroundTaskFeature
        Inherits BackgroundTask

        Public Shared Property Message As String

        Private _cancelRequested As Boolean = False

        Private _taskInstance As IBackgroundTaskInstance

        Private _deferral As BackgroundTaskDeferral

        Public Overrides Sub Register()
            Dim taskName As String = [GetType]().Name

            If Not BackgroundTaskRegistration.AllTasks.Any(Function(t) t.Value.Name = taskName) Then
                Dim builder As BackgroundTaskBuilder = New BackgroundTaskBuilder() With {
                    .Name = taskName
                }

                ' TODO WTS: Define your trigger here and set your conditions
                ' Note conditions are optional
                ' Documentation: https://docs.microsoft.com/windows/uwp/launch-resume/create-and-register-an-inproc-background-task
                builder.SetTrigger(New TimeTrigger(15, False))
                builder.AddCondition(New SystemCondition(SystemConditionType.UserPresent))

                builder.Register()
            End If
        End Sub

        Public Overrides Function RunAsyncInternal(taskInstance As IBackgroundTaskInstance) As Task
            If taskInstance Is Nothing Then
                Return Nothing
            End If

            _deferral = taskInstance.GetDeferral()

            Return Task.Run(Sub() 
                ' TODO WTS: Insert the code that should be executed in the background task here.
                ' This sample initializes a timer that counts to 100 in steps of 10.  It updates Message each time.

                ' Documentation:
                '      * General: https://docs.microsoft.com/en-us/windows/uwp/launch-resume/support-your-app-with-background-tasks
                '      * Debug: https://docs.microsoft.com/en-us/windows/uwp/launch-resume/debug-a-background-task
                '      * Monitoring: https://docs.microsoft.com/windows/uwp/launch-resume/monitor-background-task-progress-and-completion

                ' To show the background progress and message on any page in the application,
                ' subscribe to the Progress and Completed events.
                ' You can do this via "BackgroundTaskService.GetBackgroundTasksRegistration"

                _taskInstance = taskInstance
                ThreadPoolTimer.CreatePeriodicTimer(New TimerElapsedHandler(AddressOf SampleTimerCallback), TimeSpan.FromSeconds(1))
            End Sub)
        End Function

        Public Overrides Sub OnCanceled(sender As IBackgroundTaskInstance, reason As BackgroundTaskCancellationReason)
            ' TODO WTS: Insert code to handle the cancelation request here.
            ' Documentation: https://docs.microsoft.com/windows/uwp/launch-resume/handle-a-cancelled-background-task
        End Sub

        Private Sub SampleTimerCallback(timer As ThreadPoolTimer)
            If (_cancelRequested = False) AndAlso (_taskInstance.Progress < 100) Then
                _taskInstance.Progress += CUInt(10)
                Message = $"Background Task {_taskInstance.Task.Name} running"
            Else
                timer.Cancel()

                If _cancelRequested Then
                    Message = $"Background Task {_taskInstance.Task.Name} cancelled"
                Else
                    Message = $"Background Task {_taskInstance.Task.Name} finished"
                End If

                _deferral?.Complete()
            End If
        End Sub
    End Class
End Namespace 
