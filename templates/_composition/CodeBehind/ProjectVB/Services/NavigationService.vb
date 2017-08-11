Imports Windows.UI.Xaml
Imports Windows.UI.Xaml.Controls
Imports Windows.UI.Xaml.Media.Animation

Namespace Services
    Public NotInheritable Class NavigationService
        Private Sub New()
        End Sub
        Private Shared _frame As Frame

        Public Shared Property Frame() As Frame
            Get
                If _frame Is Nothing Then
                    _frame = TryCast(Window.Current.Content, Frame)
                End If

                Return _frame
            End Get
            Set
                _frame = value
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
    End Class
End Namespace
