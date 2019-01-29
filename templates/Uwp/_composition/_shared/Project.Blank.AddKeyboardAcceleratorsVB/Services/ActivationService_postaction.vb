Namespace Services
    Friend Class ActivationService
'^^
'{[{
        Public Shared AltLeftKeyboardAccelerator As KeyboardAccelerator

        Public Shared BackKeyboardAccelerator As KeyboardAccelerator
'}]}

        Public Sub New(app As App, defaultNavItem As Type, Optional shell As Lazy(Of UIElement) = Nothing)
'{[{
            AltLeftKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu)
            BackKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.GoBack)
'}]}
        End Sub

        Public Async Function ActivateAsync(activationArgs As Object) As Task
            If IsInteractive(activationArgs) Then
                Await InitializeAsync()
                If Window.Current.Content Is Nothing Then
                    Window.Current.Content = If(_shell?.Value, New Frame())
'{[{
                    AddHandler NavigationService.NavigationFailed, Function(sender, e)
                                                                        Throw e.Exception
                                                                    End Function
'}]}
                End If
            End If
        End Function

'{[{

        Private Function BuildKeyboardAccelerator(key As VirtualKey, Optional modifiers As VirtualKeyModifiers? = Nothing) As KeyboardAccelerator
            Dim KeyboardAccelerator = New KeyboardAccelerator() With {
                .Key = key
            }
            If modifiers.HasValue Then
                KeyboardAccelerator.Modifiers = modifiers
            End If

            AddHandler KeyboardAccelerator.Invoked, AddressOf OnKeyboardAcceleratorInvoked
            Return KeyboardAccelerator
        End Function

        Private Sub OnKeyboardAcceleratorInvoked(sender As KeyboardAccelerator, args As KeyboardAcceleratorInvokedEventArgs)
            Dim result = NavigationService.GoBack()
            args.Handled = result
        End Sub
'}]}
    End Class
End Namespace