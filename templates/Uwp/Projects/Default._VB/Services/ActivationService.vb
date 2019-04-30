Imports Windows.System
Imports Windows.UI.Core

Imports Param_RootNamespace.Activation

Namespace Services
    ' For more information on application activation see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/activation.vb.md
    Friend Class ActivationService
        Private ReadOnly _app As App
        Private ReadOnly _defaultNavItem As Type
        Private _shell As Lazy(Of UIElement)

        Private _lastActivationArgs As Object

        Public Sub New(app As App, defaultNavItem As Type, Optional shell As Lazy(Of UIElement) = Nothing)
            _app = app
            _shell = shell
            _defaultNavItem = defaultNavItem
        End Sub

        Public Async Function ActivateAsync(activationArgs As Object) As Task
            If IsInteractive(activationArgs) Then
                ' Initialize things like registering background task before the app is loaded
                Await InitializeAsync()

                ' Do not repeat app initialization when the Window already has content,
                ' just ensure that the window is active
                If Window.Current.Content Is Nothing Then
                    ' Create a Frame to act as the navigation context and navigate to the first page
                    Window.Current.Content = If(_shell?.Value, New Frame())
                End If
            End If

            Await HandleActivationAsync(activationArgs)
            _lastActivationArgs = activationArgs

            If IsInteractive(activationArgs) Then
                ' Ensure the current window is active
                Window.Current.Activate()

                ' Tasks after activation
                Await StartupAsync()
            End If
        End Function

        Private Async Function InitializeAsync() As Task
            Await Task.CompletedTask
        End Function

        Private Async Function HandleActivationAsync(activationArgs As Object) As Task
            Dim activationHandler = GetActivationHandlers().FirstOrDefault(Function(h) h.CanHandle(activationArgs))

            If activationHandler IsNot Nothing Then
                Await activationHandler.HandleAsync(activationArgs)
            End If

            If IsInteractive(activationArgs) Then
                Dim defaultHandler = New DefaultActivationHandler(_defaultNavItem)

                If defaultHandler.CanHandle(activationArgs) Then
                    Await defaultHandler.HandleAsync(activationArgs)
                End If
            End If
        End Function

        Private Async Function StartupAsync() As Task
            Await Task.CompletedTask
        End Function

        Private Iterator Function GetActivationHandlers() As IEnumerable(Of ActivationHandler)

            Exit Function
        End Function

        Private Function IsInteractive(args As Object) As Boolean
            Return TypeOf args Is IActivatedEventArgs
        End Function
    End Class
End Namespace
