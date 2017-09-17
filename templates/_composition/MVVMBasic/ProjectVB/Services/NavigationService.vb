Imports Windows.UI.Xaml
Imports Windows.UI.Xaml.Controls
Imports Windows.UI.Xaml.Media.Animation
Imports Windows.UI.Xaml.Navigation

Namespace Services
    Public NotInheritable Class NavigationService

        Private Sub New()
        End Sub

        Public Shared Event Navigated As NavigatedEventHandler

        Public Shared Event NavigationFailed As NavigationFailedEventHandler

        Private Shared _frame As Frame

        Public Shared Property Frame() As Frame
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

        Public Shared ReadOnly Property CanGoBack() As Boolean
            Get
                Return Frame.CanGoBack
            End Get
        End Property

        Public Shared ReadOnly Property CanGoForward() As Boolean
            Get
                Return Frame.CanGoForward
            End Get
        End Property

        Public Shared Sub GoBack()
            Frame.GoBack()
        End Sub

        Public Shared Sub GoForward()
            Frame.GoForward()
        End Sub

        Public Shared Function Navigate(pageType As Type, Optional parameter As Object = Nothing, Optional infoOverride As NavigationTransitionInfo = Nothing) As Boolean
            ' Don't open the same page multiple times
            If Frame.Content Is Nothing OrElse Not Frame.Content.GetType() Is pageType Then
                Return Frame.Navigate(pageType, parameter, infoOverride)
            Else
                Return False
            End If
        End Function

        Public Shared Function Navigate(Of T As Page)(Optional parameter As Object = Nothing, Optional infoOverride As NavigationTransitionInfo = Nothing) As Boolean
            Return Navigate(GetType(T), parameter, infoOverride)
        End Function

        Private Shared Sub RegisterFrameEvents()
            If _frame IsNot Nothing Then
                AddHandler _frame.Navigated, AddressOf Frame_Navigated
                AddHandler _frame.NavigationFailed, AddressOf Frame_NavigationFailed
            End If
        End Sub

        Private Shared Sub UnregisterFrameEvents()
            If _frame IsNot Nothing Then
                RemoveHandler _frame.Navigated, AddressOf Frame_Navigated
                RemoveHandler _frame.NavigationFailed, AddressOf Frame_NavigationFailed
            End If
        End Sub

        Private Shared Sub Frame_NavigationFailed(sender As Object, e As NavigationFailedEventArgs)
            RaiseEvent NavigationFailed(sender, e)
        End Sub 
        
        Private Shared Sub Frame_Navigated(sender As Object, e As NavigationEventArgs)
            RaiseEvent Navigated(sender, e)
        End Sub

    End Class
End Namespace