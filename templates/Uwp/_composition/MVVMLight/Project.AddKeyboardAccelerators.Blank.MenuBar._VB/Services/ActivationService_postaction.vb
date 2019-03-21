'{[{
Imports Windows.System
Imports Windows.UI.Xaml.Input
Imports Param_RootNamespace.ViewModels
'}]}
Namespace Services
    Friend Class ActivationService
'^^
'{[{
        Public Shared ReadOnly Property NavigationService As NavigationServiceEx
            Get
                Return ViewModelLocator.Current.NavigationService
            End Get
        End Property

        Public Shared ReadOnly AltLeftKeyboardAccelerator As KeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu)

        Public Shared ReadOnly BackKeyboardAccelerator As KeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.GoBack)
'}]}

        Private Async Function StartupAsync() As Task
'{[{
            If Window.Current.Content Is Nothing Then
                Window.Current.Content = If(_shell?.Value, New Frame())
                AddHandler NavigationService.NavigationFailed, Function(sender, e)
                    Throw e.Exception
                                                                End Function
            End If
'}]}
        End Function
'{[{

        Private Shared Function BuildKeyboardAccelerator(key As VirtualKey, Optional modifiers As VirtualKeyModifiers? = Nothing) As KeyboardAccelerator
            Dim keyboardAccelerator = New KeyboardAccelerator() With {
                .Key = key
            }

            If modifiers.HasValue Then
                keyboardAccelerator.Modifiers = modifiers.Value
            End If

            AddHandler keyboardAccelerator.Invoked, AddressOf OnKeyboardAcceleratorInvoked
            Return keyboardAccelerator
        End Function

        Private Shared Sub OnKeyboardAcceleratorInvoked(sender As KeyboardAccelerator, args As KeyboardAcceleratorInvokedEventArgs)
            Dim result = NavigationService.GoBack()
            args.Handled = result
        End Sub
'}]}
    End Class
End Namespace
