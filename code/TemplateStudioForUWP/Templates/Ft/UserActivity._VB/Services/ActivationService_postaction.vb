'{**
'
'**}

Namespace Services
    Friend Class ActivationService
        Private Async Function StartupAsync() As Task
            '{[{
            ' TODO WTS: This is a sample to demonstrate how to add a UserActivity. Please adapt and move this method call to where you consider convenient in your app.
            Await UserActivityService.AddSampleUserActivity()
            '}]}
            '{??{
            Await Task.CompletedTask
            '}??}
        End Function
    End Class
End Namespace
