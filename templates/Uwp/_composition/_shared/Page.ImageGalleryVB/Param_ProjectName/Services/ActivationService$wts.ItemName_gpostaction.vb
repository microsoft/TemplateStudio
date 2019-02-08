'{**
' This code block adds the call to `SampleDataService Initialize` in ActivationService of your project.
'**}
'{[{
Imports Param_RootNamespace.Core.Services
'}]}

Namespace Services
    Friend Class ActivationService
        Private Async Function InitializeAsync() As Task
'{[{
            SampleDataService.Initialize("ms-appx:///Assets")
'}]}
        End Function
    End Class
End Namespace