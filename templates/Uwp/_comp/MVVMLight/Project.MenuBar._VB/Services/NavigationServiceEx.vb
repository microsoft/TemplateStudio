Imports Windows.UI.Xaml.Media.Animation
Imports Param_RootNamespace.Helpers

Namespace Services
    Public Class NavigationServiceEx
        Public Event Navigated As NavigatedEventHandler

        Public Event NavigationFailed As NavigationFailedEventHandler

        Private ReadOnly _pages As Dictionary(Of String, Type) = New Dictionary(Of String, Type)()
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

        Public Function GoBack() As Boolean
            If CanGoBack Then
                Frame.GoBack()
                Return True
            End If

            Return False
        End Function

        Public Sub GoForward()
            Frame.GoForward()
        End Sub

        Public Function Navigate(pageKey As String, Optional parameter As Object = Nothing, Optional infoOverride As NavigationTransitionInfo = Nothing, Optional clearNavigation As Boolean = False) As Boolean
            Dim page As Type = Nothing

            SyncLock _pages

                If String.IsNullOrEmpty(pageKey) OrElse Not _pages.TryGetValue(pageKey, page) Then
                    Throw New ArgumentException(String.Format("Invalid pageKey '{0}', please provide a valid pageKey. Maybe you forgot to call NavigationService.Configure?", pageKey), NameOf(pageKey))
                End If
            End SyncLock

            If Frame.Content?.[GetType]() <> page OrElse (parameter IsNot Nothing AndAlso Not parameter.Equals(_lastParamUsed)) Then
                Frame.Tag = clearNavigation
                Dim navigationResult = Frame.Navigate(page, parameter, infoOverride)

                If navigationResult Then
                    _lastParamUsed = parameter
                End If

                Return navigationResult
            Else
                Return False
            End If
        End Function

        Public Sub Configure(key As String, pageType As Type)
            SyncLock _pages

                If _pages.ContainsKey(key) Then
                    Throw New ArgumentException(String.Format("The key {0} is already configured in NavigationService", key))
                End If

                If _pages.Any(Function(p) p.Value = pageType) Then
                    Throw New ArgumentException(String.Format("This type is already configured with key {0}", _pages.First(Function(p) p.Value = pageType).Key))
                End If

                _pages.Add(key, pageType)
            End SyncLock
        End Sub

        Public Function GetNameOfRegisteredPage(page As Type) As String
            SyncLock _pages

                If _pages.ContainsValue(page) Then
                    Return _pages.FirstOrDefault(Function(p) p.Value = page).Key
                Else
                    Throw New ArgumentException(String.Format("The page '{0}' is unknown by the NavigationService", page.Name))
                End If
            End SyncLock
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
            Dim frame = TryCast(sender, Frame)

            If frame IsNot Nothing Then
                Dim clearNavigation As Boolean = CBool(frame.Tag)

                If clearNavigation Then
                    frame.BackStack.Clear()
                End If

                RaiseEvent Navigated(sender, e)
            End If
        End Sub
    End Class
End Namespace
