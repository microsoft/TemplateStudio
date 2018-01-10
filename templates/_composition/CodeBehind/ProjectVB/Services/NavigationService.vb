Imports Windows.UI.Xaml.Media.Animation

Namespace Services
    Public Module NavigationService

        Public Event Navigated As NavigatedEventHandler

        Public Event NavigationFailed As NavigationFailedEventHandler

        Private _frame As Frame

        Public Property Frame As Frame
            Get
                If _frame Is Nothing Then
                    _frame = TryCast(Window.Current.Content, Frame)
                    RegisterFrameEvents()
                End If

                Return _frame
            End Get
            Set
                UnregisterFrameEvents()
                _frame = value
                RegisterFrameEvents()
            End Set
        End Property

        Public ReadOnly Property CanGoBack As Boolean
            Get
                Return Frame.CanGoBack
            End Get
        End Property

        Public ReadOnly Property CanGoForward As Boolean
            Get
                Return Frame.CanGoForward
            End Get
        End Property

        Public Sub GoBack()
            Frame.GoBack()
        End Sub

        Public Sub GoForward()
            Frame.GoForward()
        End Sub

        Public Function Navigate(pageType As Type, Optional parameter As Object = Nothing, Optional infoOverride As NavigationTransitionInfo = Nothing) As Boolean
            ' Don't open the same page multiple times
            If Frame.Content?.GetType IsNot pageType.GetType Then
                Return Frame.Navigate(pageType, parameter, infoOverride)
            Else
                Return False
            End If
        End Function

        Public Function Navigate(Of T As Page)(Optional parameter As Object = Nothing, Optional infoOverride As NavigationTransitionInfo = Nothing) As Boolean
            Return Navigate(GetType(T), parameter, infoOverride)
        End Function

        Private Sub RegisterFrameEvents()
            If _frame IsNot Nothing Then
                AddHandler _frame.Navigated, AddressOf Frame_Navigated
                AddHandler _frame.NavigationFailed, AddressOf Frame_NavigationFailed
            End If
        End Sub

        Private Sub UnregisterFrameEvents()
            If _frame IsNot Nothing Then
                RemoveHandler _frame.Navigated, AddressOf Frame_Navigated
                RemoveHandler _frame.NavigationFailed, AddressOf Frame_NavigationFailed
            End If
        End Sub

        Private Sub Frame_NavigationFailed(sender As Object, e As NavigationFailedEventArgs)
            RaiseEvent NavigationFailed(sender, e)
        End Sub 
        
        Private Sub Frame_Navigated(sender As Object, e As NavigationEventArgs)
            RaiseEvent Navigated(sender, e)
        End Sub
    End Module
End Namespace
