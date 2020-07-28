Imports System
Imports Windows.UI.Xaml
Imports Windows.UI.Xaml.Controls
Imports Windows.UI.Xaml.Media.Animation
Imports Windows.UI.Xaml.Navigation

Namespace Services
    Module NavigationService
        Public Event Navigated As NavigatedEventHandler

        Public Event NavigationFailed As NavigationFailedEventHandler

        Private _frame As Frame
        Private _lastParamUsed As Object

        Public Property Frame As Frame
            Get

                If _frame Is Nothing Then
                    _frame = TryCast(Window.Current.Content, Frame)
                    RegisterFrameEvents()
                End If

                Return _frame
            End Get
            Set(value As Frame)
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

        Function GoBack() As Boolean
            If CanGoBack Then
                Frame.GoBack()
                Return True
            End If

            Return False
        End Function

        Sub GoForward()
            Frame.GoForward()
        End Sub

        Function Navigate(pageType As Type, Optional parameter As Object = Nothing, Optional infoOverride As NavigationTransitionInfo = Nothing) As Boolean
            If pageType Is Nothing OrElse Not pageType.IsSubclassOf(GetType(Page)) Then
                Throw New ArgumentException($"Invalid pageType '{pageType}', please provide a valid pageType.", NameOf(pageType))
            End If

            ' Don't open the same page multiple times
            If Frame.Content?.[GetType]() <> pageType OrElse (parameter IsNot Nothing AndAlso Not parameter.Equals(_lastParamUsed)) Then
                Dim navigationResult = Frame.Navigate(pageType, parameter, infoOverride)

                If navigationResult Then
                    _lastParamUsed = parameter
                End If

                Return navigationResult
            Else
                Return False
            End If
        End Function

        Function Navigate(Of T As Page)(Optional parameter As Object = Nothing, Optional infoOverride As NavigationTransitionInfo = Nothing) As Boolean
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

        Public Sub Frame_NavigationFailed(sender As Object, e As NavigationFailedEventArgs)
            RaiseEvent NavigationFailed(sender, e)
        End Sub

        Public Sub Frame_Navigated(sender As Object, e As NavigationEventArgs)
            RaiseEvent Navigated(sender, e)
        End Sub
    End Module
End Namespace
