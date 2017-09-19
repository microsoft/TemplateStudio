'{**
' These code blocks add the ThemeSelectorService initialization to the ActivationService of your project.
'**}
Private Function InitializeAsync() As Task
    '{[{
    Await ThemeSelectorService.InitializeAsync()
    '}]}
End Function

Private Function StartupAsync() As Task
    '{[{
    ThemeSelectorService.SetRequestedTheme()
    '}]}
End Function
