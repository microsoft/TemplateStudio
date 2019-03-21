Public NotINheritable Class App
    Inherits Application

    Public Sub New()
        InitializeComponent()
        AddHandler Suspending, AddressOf OnSuspending
    End Sub

    Protected Overrides Sub OnLaunched(e As Windows.ApplicationModel.Activation.LaunchActivatedEventArgs)
        Dim rootFrame As Frame = TryCast(Window.Current.Content, Frame)

        ' Do not repeat app initialization when the Window already has content,
        ' just ensure that the window is active
        If rootFrame Is Nothing Then
            ' Create a Frame to act as the navigation context and navigate to the first page
            rootFrame = New Frame()

            AddHandler rootFrame.NavigationFailed, AddressOf OnNavigationFailed

            If e.PreviousExecutionState = ApplicationExecutionState.Terminated Then
                ' In a regular app, this is where you would
                ' Load state from previously suspended application
                ' But this should not be necessary in a test app
            End If
            ' Place the frame in the current Window
            Window.Current.Content = rootFrame
        End If

        Microsoft.VisualStudio.TestPlatform.TestExecutor.UnitTestClient.CreateDefaultUI()

        ' Ensure the current window is active
        Window.Current.Activate()

        Microsoft.VisualStudio.TestPlatform.TestExecutor.UnitTestClient.Run(e.Arguments)
    End Sub

    Private Sub OnNavigationFailed(sender As Object, e As NavigationFailedEventArgs)
        Throw New Exception("Failed to load Page " + e.SourcePageType.FullName)
    End Sub

    Private Sub OnSuspending(sender As Object, e As SuspendingEventArgs) Handles Me.Suspending
        Dim deferral As SuspendingDeferral = e.SuspendingOperation.GetDeferral()
        ' In a regular app, this is where you would
        ' Save application state and stop any background activity
        ' But this should not be necessary in a test app
        deferral.Complete()
    End Sub
End Class

' This type is defined to force the compiler to add the necessary references and allows tests to run
Public Class WinUiReference
    Inherits Windows.UI.Xaml.Controls.Button
End Class
