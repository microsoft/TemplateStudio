'{**
' These code blocks add the WindowManagerService initialization to the ActivationService of your project.
'**}

Private Async Function InitializeAsync() As Task
    '{[{
    WindowManagerService.Current.Initialize()
    '}]}
End Function
