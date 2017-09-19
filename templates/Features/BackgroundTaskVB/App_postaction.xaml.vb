'{**
' This code block adds the OnBackgroundActivated event handler to the App class of your project.
'**}
Partial NotInheritable Class App
    Inherits Application
    '^^
    '{[{
    Protected Overrides Async Sub OnBackgroundActivated(args As BackgroundActivatedEventArgs)
        Await ActivationService.ActivateAsync(args)
    End Sub
    '}]}
End Class
