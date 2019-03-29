'{**
' This code block adds the OnShareTargetActivated event handler to the App class of your project.
'**}
NotInheritable Partial Class App
    Inherits Application
    '^^
    '{[{

    Protected Overrides Async Sub OnShareTargetActivated(args As ShareTargetActivatedEventArgs)
        Await ActivationService.ActivateFromShareTargetAsync(args)
    End Sub
    '}]}
End Class
