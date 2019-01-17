'{**
' This code block includes code to show the WhatsNew control if appropriate on application startup to your project
'**}

Namespace Services
    Friend Class ActivationService
        Private Async Function StartupAsync() As Task
            '^^
            '{[{
            Await WhatsNewDisplayService.ShowIfAppropriateAsync()
            '}]}
            '{??{
            Await Task.CompletedTask
            '}??}
        End Function
    End Class
End Namespace
