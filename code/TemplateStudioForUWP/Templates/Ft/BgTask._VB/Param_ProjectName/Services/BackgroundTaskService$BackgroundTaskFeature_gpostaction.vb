'{**
' This code block adds the BackgroundTaskFeature to the method `CreateInstances()` of the BackgroundTaskService of your project.
'**}

Namespace Services
    Friend Class BackgroundTaskService
        Inherits ActivationHandler(Of BackgroundActivatedEventArgs)

        Private Shared Function CreateInstances() As IEnumerable(Of BackgroundTask)
            Dim backgroundTasks As List(Of BackgroundTask) = New List(Of BackgroundTask)()
            '^^
            '{[{
            backgroundTasks.Add(New BackgroundTaskFeature())
            '}]}
            Return backgroundTasks
        End Function
    End Class
End Namespace
