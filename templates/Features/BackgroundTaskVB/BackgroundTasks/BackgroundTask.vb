Imports System.Threading.Tasks

Imports Windows.ApplicationModel.Background

Namespace BackgroundTasks
    Public MustInherit Class BackgroundTask
        Public MustOverride Sub Register()

        Public MustOverride Function RunAsyncInternal(taskInstance As IBackgroundTaskInstance) As Task

        Public MustOverride Sub OnCanceled(sender As IBackgroundTaskInstance, reason As BackgroundTaskCancellationReason)

        Public Function Match(name As String) As Boolean
            Return name = [GetType]().Name
        End Function

        Public Function RunAsync(taskInstance As IBackgroundTaskInstance) As Task
            SubscribeToEvents(taskInstance)

            Return RunAsyncInternal(taskInstance)
        End Function

        Public Sub SubscribeToEvents(taskInstance As IBackgroundTaskInstance)
            AddHandler taskInstance.Canceled, New BackgroundTaskCanceledEventHandler(AddressOf OnCanceled)
        End Sub
    End Class
End Namespace
