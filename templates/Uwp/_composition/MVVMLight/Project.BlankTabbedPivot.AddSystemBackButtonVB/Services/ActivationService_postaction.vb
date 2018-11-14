'{[{
Imports Param_RootNamespace.ViewModels
'}]}

Namespace Services
    Friend Class ActivationService
'^^
'{[{

        Private ReadOnly Property NavigationService As NavigationServiceEx
            Get
                Return ViewModelLocator.Current.NavigationService
            End Get
        End Property

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
                    AddHandler NavigationService.Navigated, AddressOf Frame_Navigated
                    If SystemNavigationManager.GetForCurrentView() IsNot Nothing Then
                        AddHandler SystemNavigationManager.GetForCurrentView().BackRequested, AddressOf ActivationService_BackRequested
                    End If
'}]}
                End If
            End If
        End Function

'{[{
        Private Sub Frame_Navigated(sender As Object, e As NavigationEventArgs)
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = If((NavigationService.CanGoBack), AppViewBackButtonVisibility.Visible, AppViewBackButtonVisibility.Collapsed)
        End Sub

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

        Private Sub ActivationService_BackRequested(sender As Object, e As BackRequestedEventArgs)
            Dim result = NavigationService.GoBack()
            e.Handled = result
        End Sub
'}]}
    End Class
End Namespace