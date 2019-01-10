'{**
' This code block includes code to show the FirstRun control if appropriate on application startup to your project.
'**}

Namespace Services
    Friend Class ActivationService
        Private Async Function StartupAsync() As Task
            '^^
            '{[{
            Await FirstRunDisplayService.ShowIfAppropriateAsync()
            '}]}
            '{??{
            Await Task.CompletedTask
            '}??}
        End Function
    End Class
End Namespace
