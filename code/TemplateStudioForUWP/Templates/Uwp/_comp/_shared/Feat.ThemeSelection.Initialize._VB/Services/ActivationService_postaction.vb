'{**
' These code blocks add the ThemeSelectorService initialization to the ActivationService of your project.
'**}

Private Async Function InitializeAsync() As Task
    '{[{
    Await ThemeSelectorService.InitializeAsync()
    '}]}
End Function

Private Async Function StartupAsync() As Task
    '{[{
    Await ThemeSelectorService.SetRequestedThemeAsync()
    '}]}
End Function
