Imports Windows.UI.Xaml.Media.Animation
Imports Param_RootNamespace.Helpers

Namespace Services
    Public Class NavigationServiceEx

        Public Event Navigated As NavigatedEventHandler

        Public Event NavigationFailed As NavigationFailedEventHandler

        Private ReadOnly _pages As New Dictionary(Of String, Type)()

        Private Shared _frame As Frame

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

        Public Function Navigate(pageKey As String, Optional parameter As Object = Nothing, Optional infoOverride As NavigationTransitionInfo = Nothing) As Boolean
            Dim page As Type = Nothing
            SyncLock _pages
                If Not _pages.TryGetValue(pageKey, page) Then
                    Throw New ArgumentException(String.Format("ExceptionNavigationServiceExPageNotFound".GetLocalized(), pageKey), nameof(pageKey))
                End If
            End SyncLock
            Dim navigationResult = Frame.Navigate(page, parameter, infoOverride)
            Return navigationResult
        End Function

        Public Sub Configure(key As String, pageType As Type)
            SyncLock _pages
                If _pages.ContainsKey(key) Then
                    Throw New ArgumentException(String.Format("ExceptionNavigationServiceExKeyIsInNavigationService".GetLocalized(), key))
                End If

                If _pages.Any(Function(p) p.Value.Equals(pageType)) Then
                    Throw New ArgumentException(String.Format("ExceptionNavigationServiceExTypeAlreadyConfigured".GetLocalized(), _pages.First(Function(p) p.Value.Equals(pageType)).Key))
                End If

                _pages.Add(key, pageType)
            End SyncLock
        End Sub

        Public Function GetNameOfRegisteredPage(page As Type) As String
            SyncLock _pages
                If _pages.ContainsValue(page) Then
                    Return _pages.FirstOrDefault(Function(p) p.Value.Equals(page)).Key
                Else
                    Throw New ArgumentException(String.Format("ExceptionNavigationServiceExPageUnknow".GetLocalized(), page.Name))
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

        Private Sub Frame_NavigationFailed(sender As Object, e As NavigationFailedEventArgs)
            RaiseEvent NavigationFailed(sender, e)
        End Sub 
        
        Private Sub Frame_Navigated(sender As Object, e As NavigationEventArgs)
            RaiseEvent Navigated(sender, e)
        End Sub
    End Class
End Namespace
