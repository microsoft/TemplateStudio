'{**
'This code block adds the subscription to `App_EnteredBackground` to the App class of your project.
'**}

NotInheritable Partial Class App
    Inherits Application

    Public Sub New()
        InitializeComponent()

        '{[{
        AddHandler EnteredBackground, AddressOf App_EnteredBackground
        '}]}
    End Sub
    '^^
    '{[{

    Private Async Sub App_EnteredBackground(sender As Object, e As EnteredBackgroundEventArgs)
        Dim deferral = e.GetDeferral()
        Await Helpers.Singleton(Of SuspendAndResumeService).Instance.SaveStateAsync()
        deferral.Complete()
    End Sub
    '}]}
End Class
