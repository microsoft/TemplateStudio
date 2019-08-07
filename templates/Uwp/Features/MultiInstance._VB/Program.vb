Imports System
Imports Windows.ApplicationModel
Imports Windows.ApplicationModel.Activation

Public Module Program
    ' This project includes DISABLE_XAML_GENERATED_MAIN in the build properties,
    ' which prevents the build system from generating the default Main method:
    ' Sub Main(args As String())
    '     Global.Windows.UI.Xaml.Application.Start(Function(p) New App())
    ' End Sub
    ' TODO WTS: Update the logic in this method if you want to control the launching of multiple instances.
    ' You may find the `AppInstance.GetActivatedEventArgs()` useful for your app-defined logic.
    <MTAThread>
    Public Sub Main(args As String())
        ' If the platform indicates a recommended instance, use that.
        If AppInstance.RecommendedInstance IsNot Nothing Then
            AppInstance.RecommendedInstance.RedirectActivationTo()
        Else
            ' Update the logic below as appropriate for your app.
            ' Multiple instances of an app are registered using keys.
            ' Creating a unique key (as below) allows a new instance to always be created.
            ' Always using the same key will mean there's only one ever one instance.
            ' Or you can use your own logic to launch a new instance or switch to an existing one.
            Dim key = Guid.NewGuid().ToString()
            Dim instance = AppInstance.FindOrRegisterInstanceForKey(key)

            If instance.IsCurrentInstance Then
                ' If successfully registered this instance, do normal XAML initialization.
                Global.Windows.UI.Xaml.Application.Start(Function(p) New App())
            Else
                ' Some other instance has registered for this key, redirect activation to that instance.
                instance.RedirectActivationTo()
            End If
        End If
    End Sub
End Module
